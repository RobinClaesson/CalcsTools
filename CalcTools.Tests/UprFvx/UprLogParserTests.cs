using CalcTools.Core.BaseData;
using CalcTools.Core.UprFvx;
using CalcTools.Core.UprFvx.Constants;


namespace CalcTools.Tests.UprFvx;

public class UprLogParserTests
{
    private static readonly List<string> UnconventionalNames =
        ["NIDORAN♂", "NIDORAN♀", "FARFETCH’D", "MR. MIME", "PORYGON2", "HO-OH"];

    [Theory]
    [MemberData(nameof(LogTestData.PokemonTypes), MemberType = typeof(LogTestData))]
    public void ParsePokemonType_ParsesCorrectType(string logType, PokemonType expectedType)
    {
        var result = UprLogParser.ParsePokemonType(logType);

        Assert.Equal(expectedType, result);
    }

    [Fact]
    public void ParseLog_ParsesLogInformation()
    {
        // Arrange
        var log = LogTestData.BuildTestLog();

        // Act
        var result = UprLogParser.ParseLog(log);

        // Assert
        Assert.Equal("Pokemon Emerald (U)", result.BaseGame);
        Assert.Equal("FVX 1.6.1", result.RandomizerVersion);
        Assert.Equal(161080680643607UL, result.RandomSeed);
        Assert.Equal(
            "427AAQEAQT8AP8AAgECAAHkCQARAQEUAAAUAEAEAAEA/////wAAAADkBOQBAAgJ5AYEAOQAAgABAAEBAAAAAAAJAAABKAEAE1Bva2Vtb24gRW1lcmFsZCAoVSkrhZUfAAAAAA==",
            result.SettingsString);
    }

    [Fact]
    public void ParseLog_SeparatesSections()
    {
        // Arrange
        var log = LogTestData.BuildTestLog
        (
            LogTestData.LogPokemonStatisticsSection,
            LogTestData.LogMoveDataSectionWithOldName,
            LogTestData.LogPokemonMovesetsSection,
            LogTestData.LogTmMovesSection,
            LogTestData.LogTutorMovesSection
        );

        // Act
        var result = UprLogParser.ParseLog(log).Sections;

        // Assert
        Assert.Contains(LogSectionIdentifiers.TableOfContents, result.Keys);
        Assert.Contains(LogSectionIdentifiers.OverviewOfRandomization, result.Keys);
        Assert.Contains(LogSectionIdentifiers.PokemonBaseStatistics, result.Keys);
        Assert.Contains(LogSectionIdentifiers.MoveData, result.Keys);
        Assert.Contains(LogSectionIdentifiers.PokemonMoveSets, result.Keys);
        Assert.Contains(LogSectionIdentifiers.TmMoves, result.Keys);
        Assert.Contains(LogSectionIdentifiers.MoveTutorMoves, result.Keys);
        Assert.DoesNotContain(LogSectionIdentifiers.TmHmCompatability, result.Keys);
        Assert.DoesNotContain(LogSectionIdentifiers.MoveTutorCompatability, result.Keys);
        Assert.DoesNotContain(LogSectionIdentifiers.TrainerPokemon, result.Keys);
    }

    [Fact]
    public void ParseLog_ParsesOverviewOfRandomization()
    {
        // Arrange
        var log = LogTestData.BuildTestLog();

        // Act
        var result = UprLogParser.ParseLog(log).RandomizationOverview;

        // Assert
        Assert.True(result.PokemonBaseStatistics);
        Assert.True(result.PokemonTypes);
        Assert.True(result.PokemonAbilities);
        Assert.False(result.PokemonEvolutions);
        Assert.True(result.StarterPokemon);
        Assert.False(result.StaticPokemon);
        Assert.False(result.InGameTrades);
        Assert.True(result.MoveData);
        Assert.False(result.PokemonMovesets);
        Assert.False(result.TrainerPokemon);
        Assert.False(result.TrainerMovesets);
        Assert.False(result.TrainerNames);
        Assert.True(result.WildPokemon);
        Assert.False(result.TmMoves);
        Assert.False(result.TmHmCompatibility);
        Assert.False(result.MoveTutorMoves);
        Assert.False(result.MoveTutorCompatibility);
        Assert.False(result.FieldItems);
        Assert.False(result.ShopItems);
        Assert.False(result.PickupItems);
        Assert.True(result.TypeEffectiveness);
        Assert.False(result.PokemonPalettes);

        Assert.Equal("RELICANTH", result.IntroPokemon);

        var miscTweaks = result.MiscTweaks;
        Assert.True(miscTweaks.FastestText);
        Assert.True(miscTweaks.RunningShoesIndoors);
        Assert.False(miscTweaks.RandomizePcPotion);
        Assert.False(miscTweaks.GiveNationalDexAtStart);
        Assert.False(miscTweaks.FastEggHatching);
        Assert.False(miscTweaks.LowerCasePokemonNames);
        Assert.False(miscTweaks.RandomizeCatchingTutorial);
        Assert.False(miscTweaks.BanLuckyEgg);
        Assert.False(miscTweaks.RunWithoutRunningShoes);
        Assert.False(miscTweaks.ReusableTms);
        Assert.False(miscTweaks.ForgettableHms);
    }

    [Fact]
    public void ParseLog_ParsesPokemonStatistics()
    {
        // Arrange
        var log = LogTestData.BuildTestLog(LogTestData.LogPokemonStatisticsSection);

        var expectedFullInfo = new List<LogPokemon>
        {
            new()
            {
                Number = 83, Name = "FARFETCH’D", Types = ["ELECTRIC", "FIRE"], BaseStatTotal = 318,
                Hp = 65, Attack = 45, Defence = 65, SpecialAttack = 45, SpecialDefence = 49, Speed = 49,
                Abilities = ["OVERGROW"], HeldItems = []
            },
            new()
            {
                Number = 122, Name = "MR. MIME", Types = ["FLYING"], BaseStatTotal = 405,
                Hp = 80, Attack = 62, Defence = 80, SpecialAttack = 60, SpecialDefence = 63, Speed = 60,
                Abilities = ["EFFECT SPORE", "SYNCHRONIZE"], HeldItems = ["KING’S ROCK(common)"]
            },
            new()
            {
                Number = 250, Name = "HO-OH", Types = ["BUG", "WATER"], BaseStatTotal = 525,
                Hp = 80, Attack = 82, Defence = 100, SpecialAttack = 83, SpecialDefence = 100, Speed = 80,
                Abilities = ["ARENA TRAP", "STICKY HOLD"], HeldItems = ["ORAN BERRY(100%)", "LIGHT BALL(rare)"]
            }
        };

        // Act
        var result = UprLogParser.ParseLog(log).PokemonStatistics;

        // Assert
        var resultNames = result.Select(p => p.Name).ToList();
        Assert.All(UnconventionalNames, expected => Assert.Contains(expected, resultNames));

        Assert.All(expectedFullInfo, expected =>
        {
            var actual = Assert.Single(result, actual => actual.Name == expected.Name);
            Assert.Equivalent(expected, actual);
        });
    }

    [Fact]
    public void ParseLog_WhenMovesNamesAreNotRandomized_ParsesMoveData()
    {
        // Arrange
        var log = LogTestData.BuildTestLog(LogTestData.LogMoveDataSectionWithoutOldName);

        var expectedMoves = new List<LogMove>
        {
            new()
            {
                Number = 1, OldName = null, Name = "POUND", Type = "GHOST",
                Power = 50, Accuracy = 95, Pp = 15
            },
            new()
            {
                Number = 2, OldName = null, Name = "KARATE CHOP", Type = "WATER",
                Power = 85, Accuracy = 100, Pp = 25
            },
            new()
            {
                Number = 3, OldName = null, Name = "CURSE", Type = "???",
                Power = 0, Accuracy = 0, Pp = 15
            },
            new()
            {
                Number = 176, OldName = null, Name = "CONVERSION 2", Type = "FIGHTING",
                Power = 0, Accuracy = 100, Pp = 5
            }
        };

        // Act
        var result = UprLogParser.ParseLog(log).MoveData;

        // Assert
        Assert.Equivalent(expectedMoves, result);
    }

    [Fact]
    public void ParseLog_WhenMovesNamesAreRandomized_ParsesMoveData()
    {
        // Arrange
        var log = LogTestData.BuildTestLog(LogTestData.LogMoveDataSectionWithOldName);

        var expectedMoves = new List<LogMove>
        {
            new()
            {
                Number = 1, OldName = "POUND", Name = "CORE ASSAULT", Type = "NORMAL",
                Power = 40, Accuracy = 100, Pp = 35
            },
            new()
            {
                Number = 2, OldName = "KARATE CHOP", Name = "ARMBAR RUSH", Type = "FIGHTING",
                Power = 50, Accuracy = 100, Pp = 25
            },
            new()
            {
                Number = 3, OldName = "CURSE", Name = "ATTACK AURA", Type = "???",
                Power = 0, Accuracy = 0, Pp = 10
            },
            new()
            {
                Number = 176, OldName = "CONVERSION 2", Name = "GLASS GLARE", Type = "GROUND",
                Power = 0, Accuracy = 90, Pp = 40
            }
        };

        // Act
        var result = UprLogParser.ParseLog(log).MoveData;

        // Assert
        Assert.Equivalent(expectedMoves, result);
    }

    [Fact]
    public void ParseLog_ParsesPokemonMovesets()
    {
        // Arrange
        var log = LogTestData.BuildTestLog(LogTestData.LogPokemonMovesetsSection);

        var expectedFullInfo = new List<LogMoveset>
        {
            new()
            {
                Number = 32, Pokemon = "NIDORAN♂", Evolution = "NIDORINO",
                Hp = 46, Attack = 57, Defence = 40, SpecialAttack = 40, SpecialDefence = 40, Speed = 50,
                LevelUpMoves =
                [
                    new LogMovesetLevelUpMove
                    {
                        Level = 1, Name = "DRAGONBREATH", Type = "DRAGON", Category = "SPECIAL",
                        Power = 60, Pp = 20, Accuracy = 100, Stab = false
                    },
                    new LogMovesetLevelUpMove
                    {
                        Level = 20, Name = "GUILLOTINE", Type = "NORMAL", Category = "PHYSICAL",
                        Power = 1, Pp = 5, Accuracy = 30, Stab = true
                    }
                ],
                EggMoves =
                [
                    new LogMovesetMove()
                    {
                        Name = "CONVERSION 2", Type = "NORMAL", Category = "STATUS",
                        Power = 0, Pp = 15, Accuracy = 100, Stab = true
                    },
                    new LogMovesetMove()
                    {
                        Name = "MUD-SLAP", Type = "GROUND", Category = "PHYSICAL",
                        Power = 20, Pp = 10, Accuracy = 100, Stab = false
                    }
                ]
            },
            new()
            {
                Number = 122, Pokemon = "MR. MIME", Evolution = "(no evolution)",
                Hp = 40, Attack = 45, Defence = 65, SpecialAttack = 100, SpecialDefence = 120, Speed = 90,
                LevelUpMoves =
                [
                    new LogMovesetLevelUpMove
                    {
                        Level = 48, Name = "CURSE", Type = "null", Category = "STATUS",
                        Power = 0, Pp = 10, Accuracy = 0, Stab = false
                    }
                ],
                EggMoves = []
            }
        };

        // Act
        var result = UprLogParser.ParseLog(log).PokemonMovesets;

        // Assert
        var resultNames = result.Select(p => p.Pokemon).ToList();
        Assert.All(UnconventionalNames, expected => Assert.Contains(expected, resultNames));

        Assert.All(expectedFullInfo, expected =>
        {
            var actual = Assert.Single(result, actual => actual.Pokemon == expected.Pokemon);
            Assert.Equivalent(expected, actual);
        });
    }

    [Fact]
    public void ParseLog_ParsesTmMoves()
    {
        // Arrange
        var log = LogTestData.BuildTestLog(LogTestData.LogTmMovesSection);

        var expectedTms = new List<LogTmHmMove>
        {
            new() { Name = "TM01", Number = 1, Move = "HYPER BEAM" },
            new() { Name = "TM02", Number = 2, Move = "BIDE" },
            new() { Name = "TM03", Number = 3, Move = "CONVERSION 2" },
            new() { Name = "TM04", Number = 4, Move = "MUD-SLAP" },
        };

        // Act
        var result = UprLogParser.ParseLog(log).TmMoves;

        // Assert 
        Assert.Equivalent(expectedTms, result);
    }

    [Fact]
    public void ParseLog_ParsesTutorMoves()
    {
        // Arrange
        var log = LogTestData.BuildTestLog(LogTestData.LogTutorMovesSection);

        var expectedTutorMoves = new List<LogTutorMove>
        {
            new() { OldMove = "MEGA PUNCH", NewMove = "CONVERSION 2" },
            new() { OldMove = "SWORDS DANCE", NewMove = "SLUDGE" },
            new() { OldMove = "MUD-SLAP", NewMove = "ENDURE" },
            new() { OldMove = "CONVERSION 2", NewMove = "MUD-SLAP" },
        };

        // Act
        var result = UprLogParser.ParseLog(log).TutorMoves;

        // Assert
        Assert.Equivalent(expectedTutorMoves, result);
    }

    [Fact]
    public void ParseLog_ParsesTmHmCompatibility()
    {
        // Arrange
        var log = LogTestData.BuildTestLog(LogTestData.LogTmHmCompatibilitySection);

        var expectedFullInfoSpecies = new List<LogTmHmCompatabilityBySpecies>
        {
            new()
            {
                Number = 029, Pokemon = "NIDORAN♀",
                Moves =
                [
                    new LogTmHmMove { Name = "TM45", Number = 45, Move = "HAIL" },
                    new LogTmHmMove { Name = "TM48", Number = 48, Move = "CONVERSION 2" },
                    new LogTmHmMove { Name = "HM02", Number = 2, Move = "FLY" },
                    new LogTmHmMove { Name = "HM07", Number = 7, Move = "WATERFALL" }
                ]
            },
            new()
            {
                Number = 250, Pokemon = "HO-OH",
                Moves =
                [
                    new LogTmHmMove { Name = "TM45", Number = 45, Move = "HAIL" },
                    new LogTmHmMove { Name = "TM50", Number = 50, Move = "MUD-SLAP" },
                    new LogTmHmMove { Name = "HM01", Number = 1, Move = "CUT" }
                ]
            }
        };

        var expectedFullInfoTmHms = new List<LogTmHmCompatabilityByTmHm>
        {
            new()
            {
                Name = "TM01", Number = 1, Move = "MUD-SLAP",
                Species =
                [
                    new LogPokemonSpecies { Number = 29, Name = "NIDORAN♀" },
                    new LogPokemonSpecies { Number = 32, Name = "NIDORAN♂" },
                    new LogPokemonSpecies { Number = 83, Name = "FARFETCH’D" },
                ]
            },
            new()
            {
                Name = "TM02", Number = 2, Move = "CONVERSION 2",
                Species = [new LogPokemonSpecies { Number = 122, Name = "MR. MIME" }]
            },
            new()
            {
                Name = "HM08", Number = 8, Move = "DIVE",
                Species = [new LogPokemonSpecies { Number = 250, Name = "HO-OH" }]
            }
        };

        // Act
        var result = UprLogParser.ParseLog(log).TmHmCompatibility;

        // Assert
        var resultSpeciesPokemon = result.BySpecies.Select(s => s.Pokemon).ToList();
        Assert.All(UnconventionalNames, expected => Assert.Contains(expected, resultSpeciesPokemon));

        var resultTmPokemon = result.ByTmHm.SelectMany(tm => tm.Species).Select(s => s.Name).ToList();
        Assert.All(UnconventionalNames, expected => Assert.Contains(expected, resultTmPokemon));

        Assert.All(expectedFullInfoSpecies, expected =>
        {
            var actual = Assert.Single(result.BySpecies, actual =>
                actual.Pokemon == expected.Pokemon);
            Assert.Equivalent(expected, actual);
        });

        Assert.All(expectedFullInfoTmHms, expected =>
        {
            var actual = Assert.Single(result.ByTmHm, actual =>
                actual.Name == expected.Name);
            Assert.Equivalent(expected, actual);
        });
    }


    [Fact]
    public void ParseLog_ParsesMoveTutorCompatibility()
    {
        // Arrange 
        var log = LogTestData.BuildTestLog(LogTestData.LogMoveTutorCompatibilitySection);

        var expectedFullInfoSpecies = new List<LogTutorCompatibilityBySpecies>
        {
            new()
            {
                Number = 029, Pokemon = "NIDORAN♀",
                Moves = ["CONVERSION 2", "SLUDGE", "MUD-SLAP"],
            },
            new()
            {
                Number = 122, Pokemon = "MR. MIME",
                Moves = ["BIDE"],
            },
            new()
            {
                Number = 250, Pokemon = "HO-OH",
                Moves = ["SLUDGE", "FOLLOW ME", "KNOCK OFF"],
            }
        };

        var expectedFullInfoTutorMoves = new List<LogTutorCompatibilityByTutorMove>
        {
            new()
            {
                Move = "HYPER BEAM",
                Species =
                [
                    new LogPokemonSpecies { Number = 29, Name = "NIDORAN♀" },
                    new LogPokemonSpecies { Number = 32, Name = "NIDORAN♂" },
                    new LogPokemonSpecies { Number = 83, Name = "FARFETCH’D" },
                ]
            },
            new()
            {
                Move = "CONVERSION 2",
                Species =
                [
                    new LogPokemonSpecies { Number = 122, Name = "MR. MIME" },
                    new LogPokemonSpecies { Number = 233, Name = "PORYGON2" },
                ]
            },
            new()
            {
                Move = "MUD-SLAP",
                Species = [new LogPokemonSpecies { Number = 250, Name = "HO-OH" }]
            }
        };

        // Act
        var result = UprLogParser.ParseLog(log).TutorCompatibility;

        // Assert
        var resultSpeciesPokemon = result.BySpecies.Select(s => s.Pokemon).ToList();
        Assert.All(UnconventionalNames, expected => Assert.Contains(expected, resultSpeciesPokemon));

        var resultTutorPokemon = result.ByTutorMove.SelectMany(tm => tm.Species).Select(s => s.Name).ToList();
        Assert.All(UnconventionalNames, expected => Assert.Contains(expected, resultTutorPokemon));

        Assert.All(expectedFullInfoSpecies, expected =>
        {
            var actual = Assert.Single(result.BySpecies, actual =>
                actual.Pokemon == expected.Pokemon);
            Assert.Equivalent(expected, actual);
        });

        Assert.All(expectedFullInfoTutorMoves, expected =>
        {
            var actual = Assert.Single(result.ByTutorMove, actual =>
                actual.Move == expected.Move);
            Assert.Equivalent(expected, actual);
        });
    }

    [Fact]
    public void ParseLog_ParsesTrainerPokemon()
    {
        // Arrange
        var log = LogTestData.BuildTestLog(LogTestData.LogTrainerPokemonSection);

        var expectedTrainers = new List<LogTrainer>
        {
            new()
            {
                Number = 1, Class = "HIKER", Name = "SAWYER", Id = "310058", BattleStyle = "Single",
                Pokemon =
                [
                    new LogTrainerPokemon { Pokemon = "FARFETCH’D", Level = 21, Item = null }
                ]
            },
            new()
            {
                Number = 9, Class = "[PK][MN] BREEDER", Name = "GABRIELLE", Id = "310198", BattleStyle = null,
                Pokemon =
                [
                    new LogTrainerPokemon { Pokemon = "NIDORAN♂", Level = 46, Item = null },
                    new LogTrainerPokemon { Pokemon = "NIDORAN♀", Level = 48, Item = null },
                    new LogTrainerPokemon { Pokemon = "MR. MIME", Level = 46, Item = null },
                    new LogTrainerPokemon { Pokemon = "PORYGON2", Level = 48, Item = null },
                    new LogTrainerPokemon { Pokemon = "HO-OH", Level = 49, Item = "KING’S ROCK" }
                ]
            },
            new()
            {
                Number = 34, Class = "LEADER", Name = "TATE&LIZA", Id = "310580", BattleStyle = "Double",
                Pokemon =
                [
                    new LogTrainerPokemon
                    {
                        Pokemon = "MIGHTYENA", Level = 41, Item = null, Ability = "INTIMIDATE",
                        Moves = ["HYPER BEAM", "MUD-SLAP", "CONVERSION 2", "DOUBLE TEAM"]
                    },
                    new LogTrainerPokemon
                    {
                        Pokemon = "KINGDRA", Level = 46, Item = "CHESTO BERRY", Ability = "SWIFT SWIM",
                        Moves = ["HYDRO PUMP", "FRUSTRATION", "SLEEP TALK", "ENDURE"]
                    }
                ]
            }
        };

        // Act
        var result = UprLogParser.ParseLog(log).TrainerPokemon;

        // Assert
        Assert.Equivalent(expectedTrainers, result);
    }
}