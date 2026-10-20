using System.Text.RegularExpressions;
using CalcTools.Core.HzlaCalc;
using CalcTools.Core.UprFvx.Constants;

namespace CalcTools.Core.UprFvx;

public static partial class UprLogParser
{
    public static async Task<RandomizerLog> ParseLogAsync(Stream logStream)
    {
        using var reader = new StreamReader(logStream);
        var log = await reader.ReadToEndAsync();
        return ParseLog(log);
    }

    public static RandomizerLog ParseLog(string logContent)
    {
        var sections = DividerRegex()
            .Split(logContent)
            .Where(s => !string.IsNullOrWhiteSpace(s))
            .ToList();

        var logInfo = ParseKeyValuePairs(sections[1]);
        var categorizedSections = sections.Skip(2).Select(CategorizeSection)
            .Where(s => s != null)
            .Cast<LogSection>()
            .ToDictionary(
                s => s.Identifier,
                s => s
            );

        return new RandomizerLog
        {
            BaseGame = logInfo[LogInfoKeys.BaseGame],
            RandomizerVersion = logInfo[LogInfoKeys.RandomizerVersion],
            RandomSeed = ulong.Parse(logInfo[LogInfoKeys.RandomSeed]),
            SettingsString = logInfo[LogInfoKeys.SettingsString],
            RandomizationOverview = ParseOverview(categorizedSections),
            PokemonStatistics = ParseLogPokemon(categorizedSections),
            MoveData = ParseMoveData(categorizedSections),
            PokemonMovesets = ParseMovesets(categorizedSections),
            TmMoves = ParseTmMoves(categorizedSections),
            TutorMoves = ParseTutorMoves(categorizedSections),
            TmHmCompatibility = ParseTmHmCompatibility(categorizedSections),
            TutorCompatibility = ParseTutorCompatibility(categorizedSections),
            Sections = categorizedSections,
        };
    }

    public static PokemonType ParsePokemonType(string logType) => logType switch
    {
        "???" or "null" => PokemonType.Unkown,
        _ => Enum.Parse<PokemonType>(logType, ignoreCase: true)
    };

    private static LogSection? CategorizeSection(string section)
    {
        var sectionTitleMatch = SectionTitleRegex().Match(section);

        if (!sectionTitleMatch.Success)
            return null;

        var titleEndIndex = sectionTitleMatch.Index + sectionTitleMatch.Length;

        return new LogSection
        {
            Title = sectionTitleMatch.Groups["title"].Value,
            Identifier = sectionTitleMatch.Groups["identifier"].Value,
            Content = section[titleEndIndex..]
        };
    }

    private static RandomizationOverview ParseOverview(Dictionary<string, LogSection> sections)
    {
        var overviewSection = sections[LogSectionIdentifiers.OverviewOfRandomization];
        var info = ParseKeyValuePairs(overviewSection.Content);

        return new RandomizationOverview
        {
            PokemonBaseStatistics = WasRandomized(RandomizationOverviewKeys.PokemonBaseStatistics),
            PokemonTypes = WasRandomized(RandomizationOverviewKeys.PokemonTypes),
            PokemonAbilities = WasRandomized(RandomizationOverviewKeys.PokemonAbilities),
            PokemonEvolutions = WasRandomized(RandomizationOverviewKeys.PokemonEvolutions),
            StarterPokemon = WasRandomized(RandomizationOverviewKeys.StarterPokemon),
            StaticPokemon = WasRandomized(RandomizationOverviewKeys.StaticPokemon),
            InGameTrades = WasRandomized(RandomizationOverviewKeys.InGameTrades),
            MoveData = WasRandomized(RandomizationOverviewKeys.MoveData),
            PokemonMovesets = WasRandomized(RandomizationOverviewKeys.PokemonMovesets),
            TrainerPokemon = WasRandomized(RandomizationOverviewKeys.TrainerPokemon),
            TrainerMovesets = WasRandomized(RandomizationOverviewKeys.TrainerMovesets),
            TrainerNames = WasRandomized(RandomizationOverviewKeys.TrainerNames),
            WildPokemon = WasRandomized(RandomizationOverviewKeys.WildPokemon),
            TmMoves = WasRandomized(RandomizationOverviewKeys.TmMoves),
            TmHmCompatibility = WasRandomized(RandomizationOverviewKeys.TmHmCompatibility),
            MoveTutorMoves = WasRandomized(RandomizationOverviewKeys.MoveTutorMoves),
            MoveTutorCompatibility = WasRandomized(RandomizationOverviewKeys.MoveTutorCompatibility),
            FieldItems = WasRandomized(RandomizationOverviewKeys.FieldItems),
            ShopItems = WasRandomized(RandomizationOverviewKeys.ShopItems),
            PickupItems = WasRandomized(RandomizationOverviewKeys.PickupItems),
            TypeEffectiveness = WasRandomized(RandomizationOverviewKeys.TypeEffectiveness),
            PokemonPalettes = WasRandomized(RandomizationOverviewKeys.PokemonPalettes),
            IntroPokemon = info[RandomizationOverviewKeys.IntroPokemon],
            MiscTweaks = ParseMiscTweaks(overviewSection)
        };

        bool WasRandomized(string key) => info[key].Equals(
            RandomizationOverviewKeys.RandomizedChanged,
            StringComparison.InvariantCultureIgnoreCase
        );
    }

    private static MiscTweaks ParseMiscTweaks(LogSection overviewSection)
    {
        return new MiscTweaks
        {
            FastestText = HasTweak(MiscTweakTexts.FastestText),
            RunningShoesIndoors = HasTweak(MiscTweakTexts.RunningShoesIndoors),
            RandomizePcPotion = HasTweak(MiscTweakTexts.RandomizePcPotion),
            GiveNationalDexAtStart = HasTweak(MiscTweakTexts.GiveNationalDexAtStart),
            FastEggHatching = HasTweak(MiscTweakTexts.FastEggHatching),
            LowerCasePokemonNames = HasTweak(MiscTweakTexts.LowerCasePokemonNames),
            RandomizeCatchingTutorial = HasTweak(MiscTweakTexts.RandomizeCatchingTutorial),
            BanLuckyEgg = HasTweak(MiscTweakTexts.BanLuckyEgg),
            RunWithoutRunningShoes = HasTweak(MiscTweakTexts.RunWithoutRunningShoes),
            ReusableTms = HasTweak(MiscTweakTexts.ReusableTms),
            ForgettableHms = HasTweak(MiscTweakTexts.ForgettableHms),
            NoEVsFromPokemon = HasTweak(MiscTweakTexts.NoEVsFromPokemon),
        };

        bool HasTweak(string tweakText) => overviewSection.Content
            .Contains(tweakText, StringComparison.InvariantCultureIgnoreCase);
    }

    private static List<LogPokemon> ParseLogPokemon(Dictionary<string, LogSection> sections) =>
        sections.TryGetValue(LogSectionIdentifiers.PokemonBaseStatistics, out var section)
            ? [.. PokemonStatisticsRegex().Matches(section.Content).Select(BuildLogPokemon)]
            : [];

    private static List<LogMove> ParseMoveData(Dictionary<string, LogSection> sections) =>
        sections.TryGetValue(LogSectionIdentifiers.MoveData, out var section)
            ? [.. MoveDataRegex().Matches(section.Content).Select(BuildLogMove)]
            : [];

    private static List<LogMoveset> ParseMovesets(Dictionary<string, LogSection> sections) =>
        sections.TryGetValue(LogSectionIdentifiers.PokemonMoveSets, out var section)
            ?
            [
                .. EmptyLineRegex().Split(section.Content)
                    .Where(s => !string.IsNullOrWhiteSpace(s))
                    .Select(ParseMovesetSubsection)
            ]
            : [];

    private static LogMoveset ParseMovesetSubsection(string subsection)
    {
        var info = MovesetPokemonInfoRegex().Match(subsection);
        var stats = MovesetPokemonStatsRegex().Match(subsection);
        var moves = MovesetMovesRegex().Matches(subsection)
            .ToLookup(m => m.Groups["level"].Success, m => m);

        return new LogMoveset
        {
            Number = int.Parse(info.Groups["number"].Value),
            Name = info.Groups["name"].Value,
            Evolution = info.Groups["evolution"].Value,
            Hp = int.Parse(stats.Groups["stat"].Captures[0].Value),
            Attack = int.Parse(stats.Groups["stat"].Captures[1].Value),
            Defence = int.Parse(stats.Groups["stat"].Captures[2].Value),
            SpecialAttack = int.Parse(stats.Groups["stat"].Captures[3].Value),
            SpecialDefence = int.Parse(stats.Groups["stat"].Captures[4].Value),
            Speed = int.Parse(stats.Groups["stat"].Captures[5].Value),
            LevelUpMoves = [.. moves[true].Select(BuildLogMovesetLevelUpMove)],
            EggMoves = [.. moves[false].Select(BuildLogMovesetMove)]
        };
    }

    private static List<LogTmHmMove> ParseTmMoves(Dictionary<string, LogSection> sections) =>
        sections.TryGetValue(LogSectionIdentifiers.TmMoves, out var section)
            ? [.. TmMoveRegex().Matches(section.Content).Select(BuildLogTmMove)]
            : [];

    private static List<LogTutorMove> ParseTutorMoves(Dictionary<string, LogSection> sections) =>
        sections.TryGetValue(LogSectionIdentifiers.MoveTutorMoves, out var section)
            ? [.. TutorMoveRegex().Matches(section.Content).Select(BuildLogTutorMove)]
            : [];

    private static LogTmHmCompatibility ParseTmHmCompatibility(Dictionary<string, LogSection> sections)
    {
        if (!sections.TryGetValue(LogSectionIdentifiers.TmHmCompatability, out var section))
        {
            return new LogTmHmCompatibility();
        }

        var subsections = CompatibilitySubsectionRegex().Matches(section.Content);

        var bySpeciesSubsection = subsections
            .FirstOrDefault(m => m.Groups["by"].Value == LogCompatibilitySubsectionTitles.BySpecies)?
            .Groups["content"].Value;

        var byTmHmSubsection = subsections
            .FirstOrDefault(m => m.Groups["by"].Value == LogCompatibilitySubsectionTitles.ByTmHm)?
            .Groups["content"].Value;

        return new LogTmHmCompatibility
        {
            BySpecies =
            [
                .. TmHmCompatibilityBySpeciesRegex().Matches(bySpeciesSubsection ?? string.Empty)
                    .Select(BuildLogTmHmCompatabilityBySpecies)
            ],
            ByTmHm =
            [
                .. TmHmCompatibilityByTmHmRegex().Matches(byTmHmSubsection ?? string.Empty)
                    .Select(BuildLogTmHmCompatabilityByTmHm)
            ]
        };
    }

    private static LogTutorCompatibility ParseTutorCompatibility(Dictionary<string, LogSection> sections)
    {
        if (!sections.TryGetValue(LogSectionIdentifiers.MoveTutorCompatability, out var section))
        {
            return new LogTutorCompatibility();
        }

        var subsections = CompatibilitySubsectionRegex().Matches(section.Content);

        var bySpeciesSubsection = subsections
            .FirstOrDefault(m => m.Groups["by"].Value == LogCompatibilitySubsectionTitles.BySpecies)?
            .Groups["content"].Value;

        var byTutorMoveSubsection = subsections
            .FirstOrDefault(m => m.Groups["by"].Value == LogCompatibilitySubsectionTitles.ByTutorMove)?
            .Groups["content"].Value;

        return new LogTutorCompatibility
        {
            BySpecies =
            [
                .. TutorCompatibilityBySpeciesRegex().Matches(bySpeciesSubsection ?? string.Empty)
                    .Select(BuildLogTutorCompatibilityBySpecies)
            ],
            ByTutorMove =
            [
                .. TutorCompatibilityByTutorMoveRegex().Matches(byTutorMoveSubsection ?? string.Empty)
                    .Select(BuildLogTutorCompatibilityByTutorMove)
            ]
        };
    }

    private static Dictionary<string, string> ParseKeyValuePairs(string section) => KeyValuePairRegex()
        .Matches(section)
        .ToDictionary(
            m => m.Groups["key"].Value,
            m => m.Groups["value"].Value.Trim(),
            StringComparer.InvariantCultureIgnoreCase
        );

    private static LogPokemon BuildLogPokemon(Match match) => new()
    {
        Number = int.Parse(match.Groups["number"].Value),
        Name = match.Groups["name"].Value,
        Types = [.. match.Groups["type"].Captures.Select(c => c.Value)],
        BaseStatTotal = int.Parse(match.Groups["stat"].Captures[0].Value),
        Hp = int.Parse(match.Groups["stat"].Captures[1].Value),
        Attack = int.Parse(match.Groups["stat"].Captures[2].Value),
        Defence = int.Parse(match.Groups["stat"].Captures[3].Value),
        SpecialAttack = int.Parse(match.Groups["stat"].Captures[4].Value),
        SpecialDefence = int.Parse(match.Groups["stat"].Captures[5].Value),
        Speed = int.Parse(match.Groups["stat"].Captures[6].Value),
        Abilities = [.. match.Groups["ability"].Captures.Select(c => c.Value)],
        HeldItems = [.. match.Groups["item"].Captures.Select(c => c.Value)]
    };

    private static LogMovesetLevelUpMove BuildLogMovesetLevelUpMove(Match match) => new()
    {
        Level = int.Parse(match.Groups["level"].Value),
        Name = match.Groups["name"].Value,
        Type = match.Groups["type"].Value,
        Category = match.Groups["category"].Value,
        Power = int.TryParse(match.Groups["power"].Value, out var power) ? power : 0,
        Pp = int.Parse(match.Groups["pp"].Value),
        Accuracy = int.Parse(match.Groups["accuracy"].Value),
        Stab = match.Groups["stab"].Success
    };

    private static LogMovesetMove BuildLogMovesetMove(Match match) => new()
    {
        Name = match.Groups["name"].Value,
        Type = match.Groups["type"].Value,
        Category = match.Groups["category"].Value,
        Power = int.TryParse(match.Groups["power"].Value, out var power) ? power : 0,
        Pp = int.Parse(match.Groups["pp"].Value),
        Accuracy = int.Parse(match.Groups["accuracy"].Value),
        Stab = match.Groups["stab"].Success
    };

    private static LogMove BuildLogMove(Match match) => new()
    {
        Number = int.Parse(match.Groups["number"].Value),
        OldName = match.Groups["old_name"].Success ? match.Groups["old_name"].Value : null,
        Name = match.Groups["name"].Value,
        Type = match.Groups["type"].Value,
        Power = int.Parse(match.Groups["power"].Value),
        Accuracy = int.Parse(match.Groups["accuracy"].Value),
        Pp = int.Parse(match.Groups["pp"].Value),
    };

    private static LogTmHmMove BuildLogTmMove(Match match) => new()
    {
        Number = match.Groups["number"].Value,
        Move = match.Groups["move"].Value,
    };

    private static LogTutorMove BuildLogTutorMove(Match match) => new()
    {
        OldMove = match.Groups["old_move"].Value,
        NewMove = match.Groups["new_move"].Value,
    };

    private static LogTmHmCompatabilityBySpecies BuildLogTmHmCompatabilityBySpecies(Match match) => new()
    {
        Number = int.Parse(match.Groups["pokemon_number"].Value),
        Pokemon = match.Groups["pokemon"].Value,
        Moves =
        [
            .. match.Groups["tm_number"].Captures.Zip(match.Groups["move"].Captures,
                (number, move) => new LogTmHmMove { Number = number.Value, Move = move.Value })
        ]
    };

    private static LogTmHmCompatabilityByTmHm BuildLogTmHmCompatabilityByTmHm(Match match) => new()
    {
        Number = match.Groups["tm_number"].Value,
        Move = match.Groups["move"].Value,
        Species = BuildLogPokemonSpecies(match)
    };

    private static LogTutorCompatibilityBySpecies BuildLogTutorCompatibilityBySpecies(Match match) => new()
    {
        Number = int.Parse(match.Groups["pokemon_number"].Value),
        Pokemon = match.Groups["pokemon"].Value,
        Moves = [.. match.Groups["move"].Captures.Select(c => c.Value)]
    };

    private static LogTutorCompatibilityByTutorMove BuildLogTutorCompatibilityByTutorMove(Match match) => new()
    {
        Move = match.Groups["move"].Value,
        Species = BuildLogPokemonSpecies(match)
    };

    private static List<LogPokemonSpecies> BuildLogPokemonSpecies(Match match) =>
    [
        .. match.Groups["pokemon_number"].Captures.Zip(match.Groups["pokemon"].Captures,
            (number, pokemon) => new LogPokemonSpecies
            {
                Number = int.Parse(number.Value),
                Name = pokemon.Value
            })
    ];

    [GeneratedRegex("^=+$", RegexOptions.Multiline)]
    private static partial Regex DividerRegex();

    [GeneratedRegex("^(?<key>.+):(?<value>.+)$", RegexOptions.Multiline)]
    private static partial Regex KeyValuePairRegex();

    [GeneratedRegex(@"^\(\s+(?<title>.+)\s+{(?<identifier>.+)}\s+\)$", RegexOptions.Multiline)]
    private static partial Regex SectionTitleRegex();

    [GeneratedRegex(@"(?:\r?\n)[ \t]*(?:\r?\n)+")]
    private static partial Regex EmptyLineRegex();

    [GeneratedRegex(@"(?<number>\d+)\s+(?<name>.+)\s+->\s+(?<evolution>.+)")]
    private static partial Regex MovesetPokemonInfoRegex();

    [GeneratedRegex(@"^(?:\s*(?<stat>\d+)\|?){6}\s*$", RegexOptions.Multiline)]
    private static partial Regex MovesetPokemonStatsRegex();

    [GeneratedRegex(
        @"(?<level>\d+)?[\s:]+(?<name>\w[\w\s-]+\w)\s*\|\s*(?<type>\w+)\s*\|\s*(?<category>\w+)\s*\|\s*POW=\s*(?<power>[\d-]+)\s*\|\s*PP=\s*(?<pp>\d+)\s*\|\s*ACC=\s*(?<accuracy>\d+)%\s*(?<stab>\(STAB\))?$",
        RegexOptions.Multiline)]
    private static partial Regex MovesetMovesRegex();

    [GeneratedRegex(
        @"^\s*(?<number>\d+)\|(?<name>\w[^|\s]+\w)\s*\|(?<type>\w+)(?:\/(?<type>\w+))?\s*\|(?:\s*(?<stat>\d+)\|){7}(?<ability>\w[\w\s]+\w)\s*\|(?:(?<ability>\w[\w\s]+\w)|-+)\s*\|(?:(?<item>\w[\w\s]+\([\w%]+\))(?:, )?)*$",
        RegexOptions.Multiline)]
    private static partial Regex PokemonStatisticsRegex();

    [GeneratedRegex(
        @"^(?<number>\d+)\s*\|(?:(?<old_name>\w[\w\s-]+\w)\s*\|)?(?<name>\w[\w\s-]+\w)\s*\|(?<type>[\w?]+)\s*\|(?<power>\d+)\s*\|(?<accuracy>\d+)%\s*\|(?<pp>\d+)\s*$",
        RegexOptions.Multiline)]
    private static partial Regex MoveDataRegex();

    [GeneratedRegex(@"^(?<number>(?:TM|HM)\d+)\s*(?<move>\w[\w -]+\w)\s*$", RegexOptions.Multiline)]
    private static partial Regex TmMoveRegex();

    [GeneratedRegex(@"^(?<old_move>\w[\w -]+\w)\s*->\s*(?<new_move>\w[\w -]+\w)\s*$", RegexOptions.Multiline)]
    private static partial Regex TutorMoveRegex();

    [GeneratedRegex(@"--(?<by>[\w\s\/]+):--(?<content>.*?)(?=--[\w\s\/]+:--|$)", RegexOptions.Singleline)]
    private static partial Regex CompatibilitySubsectionRegex();

    [GeneratedRegex(
        @"#(?<pokemon_number>\d+)\s*(?<pokemon>\w[^\s]+)(?:\s*(?<tm_number>(?:TM|HM)\d+)\s*(?<move>\w[\w -]+\w),?)*")]
    private static partial Regex TmHmCompatibilityBySpeciesRegex();

    [GeneratedRegex(
        @"(?<tm_number>(?:TM|HM)\d+)\s*(?<move>\w[\w -]+\w)(?:\s*#(?<pokemon_number>\d+)\s*(?<pokemon>\w[^\s,]+),?)*")]
    private static partial Regex TmHmCompatibilityByTmHmRegex();

    [GeneratedRegex(@"#(?<pokemon_number>\d+)\s*(?<pokemon>\w[^\s]+)(?:\s*(?<move>\w[\w -]+\w),?)*")]
    private static partial Regex TutorCompatibilityBySpeciesRegex();

    [GeneratedRegex(
        @"(?<move>\w[\w -]+\w)(?:\s*#(?<pokemon_number>\d+)\s*(?<pokemon>\w[^\s,]+),?)*")]
    private static partial Regex TutorCompatibilityByTutorMoveRegex();
}