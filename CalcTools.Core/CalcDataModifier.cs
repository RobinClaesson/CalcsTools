using CalcTools.Core.Extensions;
using CalcTools.Core.HzlaCalc;
using CalcTools.Core.UprFvx;

namespace CalcTools.Core;

public static class CalcDataModifier
{
    public static void ApplyRandomizerChanges(CalcData calcData, RandomizerLog randomizerLog)
    {
        calcData.Pokemons = calcData.Pokemons.ToDictionary(StringComparer.InvariantCultureIgnoreCase);
        calcData.Moves = calcData.Moves.ToDictionary(StringComparer.InvariantCultureIgnoreCase);
        calcData.FormattedSets = calcData.FormattedSets.ToDictionary(StringComparer.InvariantCultureIgnoreCase);

        FindAndSetApostrophe(calcData);

        ApplyRandomizerPokemonChanges(calcData, randomizerLog);
        ApplyRandomizerMovesetChanges(calcData, randomizerLog);
        ApplyRandomizerMoveChanges(calcData, randomizerLog);
        ApplyRandomizerTmChanges(calcData, randomizerLog);
    }

    public static void FixTrainerNames(CalcData calcData) => calcData.FormattedSets.Values.ToList()
        .ForEach(sets => sets.Keys.Where(k => k.HasUnicodeError())
            .ToList().ForEach(k => sets.UpdateKey(k, k.FixUnicodeCharachters())));

    private static void FindAndSetApostrophe(CalcData calcData)
    {
        var apostropheChar = calcData.Pokemons.Keys
            .Select(name => name.FirstOrDefault(c => c is '\u2019' or '\u2018' or '\u02BC'))
            .FirstOrDefault(c => c is not '\0');

        if (apostropheChar is not '\0')
            StringExtensions.CalcApostrophe = apostropheChar;
    }

    private static void ApplyRandomizerPokemonChanges(CalcData calcData, RandomizerLog randomizerLog)
    {
        foreach (var logPokemon in randomizerLog.PokemonStatistics)
        {
            var key = logPokemon.Name.NormalizePokemonName();

            var calcPokemon = calcData.Pokemons[key];
            calcPokemon.Types = [.. logPokemon.Types.Select(UprLogParser.ParsePokemonType)];
            calcPokemon.BaseStats.Hp = logPokemon.Hp;
            calcPokemon.BaseStats.Attack = logPokemon.Attack;
            calcPokemon.BaseStats.Defence = logPokemon.Defence;
            calcPokemon.BaseStats.SpecialAttack = logPokemon.SpecialAttack;
            calcPokemon.BaseStats.SpecialDefence = logPokemon.SpecialDefence;
            calcPokemon.BaseStats.Speed = logPokemon.Speed;

            if (!randomizerLog.RandomizationOverview.PokemonAbilities ||
                !calcData.FormattedSets.TryGetValue(key, out var sets))
                continue;

            //For when Pokémon Abilities were randomized but Trainer Movesets were not:
            //Set ability in formatted sets to ability 1.
            //This might not always be true, but it's better than having the original ability which is always wrong.   
            var ability = logPokemon.Abilities[0].ToTitleCase();
            sets.Values.ToList().ForEach(s => s.Ability = ability);
        }
    }

    private static void ApplyRandomizerMoveChanges(CalcData calcData, RandomizerLog randomizerLog)
    {
        foreach (var logMove in randomizerLog.MoveData)
        {
            var logMoveName = logMove.OldName ?? logMove.Name;
            var key = CheckMismatches(logMoveName);

            var calcMove = calcData.Moves[key];
            calcMove.BasePower = logMove.Power;
            calcMove.Type = UprLogParser.ParsePokemonType(logMove.Type);

            if (logMove.OldName is null)
                continue;

            var newName = logMove.Name.ToTitleCase();
            calcData.Moves.UpdateKey(key, newName);

            foreach (var learnsetInfo in calcData.Pokemons.Values.Select(pkm => pkm.LearnsetInfo))
            {
                learnsetInfo.TMs.Replace(key, newName, StringComparer.InvariantCultureIgnoreCase);
                learnsetInfo.TMs.Replace(logMove.OldName, newName, StringComparer.InvariantCultureIgnoreCase);
                learnsetInfo.Learnsets.Where(ls =>
                    ls.Move.Equals(key, StringComparison.InvariantCultureIgnoreCase) ||
                    ls.Move.Equals(logMove.OldName, StringComparison.InvariantCultureIgnoreCase)
                ).ToList().ForEach(ls => ls.Move = newName);
            }

            foreach (var set in calcData.FormattedSets.Values.SelectMany(fs => fs.Values))
            {
                set.Moves.Replace(key, newName, StringComparer.InvariantCultureIgnoreCase);
                set.Moves.Replace(logMove.OldName, newName, StringComparer.InvariantCultureIgnoreCase);
            }
        }
    }

    private static void ApplyRandomizerMovesetChanges(CalcData calcData, RandomizerLog randomizerLog)
    {
        foreach (var logLearnset in randomizerLog.PokemonMovesets)
        {
            var key = logLearnset.Pokemon.NormalizePokemonName();

            try
            {
                calcData.Pokemons[key].LearnsetInfo.Learnsets =
                [
                    .. logLearnset.LevelUpMoves
                        .Select(logMove => new CalcLevelUpMove
                        {
                            Level = logMove.Level,
                            Move = CheckMismatches(logMove.Name).ToTitleCase()
                        })
                ];
            }
            catch
            {
                Console.WriteLine($"Key: {key}");
                var target = calcData.Pokemons.Keys.First(k => k.Contains("Farfetch"));
                Console.WriteLine($"Target: {target}");
                throw;
            }
        }
    }

    private static void ApplyRandomizerTmChanges(CalcData calcData, RandomizerLog randomizerLog)
    {
        if (randomizerLog.RandomizationOverview is { TmMoves: true, TmHmCompatibility: false })
        {
            ApplyRandomizerTmNameChanges(calcData, randomizerLog);
            return;
        }

        foreach (var logSpecies in randomizerLog.TmHmCompatibility.BySpecies)
        {
            var key = logSpecies.Pokemon.NormalizePokemonName();

            calcData.Pokemons[key].LearnsetInfo.TMs =
            [
                .. logSpecies.Moves.Select(tm => tm.Move)
                    .Select(CheckMismatches)
                    .Select(s => s.ToTitleCase())
            ];
        }
    }

    private static void ApplyRandomizerTmNameChanges(CalcData calcData, RandomizerLog randomizerLog)
    {
        var gameTms = GamesInfo.GetTmMoves(3);
        var gameHms = GamesInfo.GetHmMoves(3)
            .Select(hm => hm.Value)
            .ToList();

        var movesMap = randomizerLog.TmMoves
            .ToDictionary(
                tm => CheckMismatches(gameTms[tm.Number]),
                tm => CheckMismatches(tm.Move).ToTitleCase(),
                StringComparer.InvariantCultureIgnoreCase);

        foreach (var calcPokemon in calcData.Pokemons.Values)
        {
            calcPokemon.LearnsetInfo.TMs =
                [.. calcPokemon.LearnsetInfo.TMs.Select(CheckMismatches).Select(TranslateMove)];
        }

        return;

        string TranslateMove(string move) => gameHms.Contains(move, StringComparer.InvariantCultureIgnoreCase)
            ? move
            : movesMap[CheckMismatches(move)];
    }

    private static string CheckMismatches(string move) => KnownMoveMismatches.GetValueOrDefault(move, move);

    private static readonly Dictionary<string, string> KnownMoveMismatches =
        new(StringComparer.InvariantCultureIgnoreCase)
        {
            { "DOUBLESLAP", "Double Slap" }, { "THUNDERPUNCH", "Thunder Punch" }, { "FIREPUNCH", "Fire Punch" },
            { "ICEPUNCH", "Ice Punch" }, { "VICEGRIP", "Vise Grip" }, { "SAND-ATTACK", "Sand Attack" },
            { "SONICBOOM", "Sonic Boom" }, { "BUBBLEBEAM", "Bubble Beam" }, { "SOLARBEAM", "Solar Beam" },
            { "POISONPOWDER", "Poison Powder" }, { "THUNDERSHOCK", "Thunder Shock" },
            { "SELFDESTRUCT", "Self-Destruct" }, { "SOFTBOILED", "Soft-Boiled" }, { "HI JUMP KICK", "High Jump Kick" },
            { "FAINT ATTACK", "Feint Attack" }, { "DYNAMICPUNCH", "Dynamic Punch" },
            { "DRAGONBREATH", "Dragon Breath" }, { "EXTREMESPEED", "Extreme Speed" },
            { "ANCIENTPOWER", "Ancient Power" }, { "SMELLINGSALT", "Smelling Salts" },
            { "FEATHERDANCE", "Feather Dance" }, { "GRASSWHISTLE", "Grass Whistle" }
        };
}