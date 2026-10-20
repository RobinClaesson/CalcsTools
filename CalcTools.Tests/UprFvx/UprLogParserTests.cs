using CalcTools.Core.UprFvx;
using CalcTools.Core.UprFvx.Constants;
using PokemonType = CalcTools.Core.HzlaCalc.PokemonType;

namespace CalcTools.Tests.UprFvx;

public class UprLogParserTests
{
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

        var expectedPokemon = new List<LogPokemon>
        {
            new()
            {
                Number = 1, Name = "BULBASAUR", Types = ["ELECTRIC", "FIRE"], BaseStatTotal = 318,
                Hp = 65, Attack = 45, Defence = 65, SpecialAttack = 45, SpecialDefence = 49, Speed = 49,
                Abilities = ["OVERGROW"], HeldItems = []
            },
            new()
            {
                Number = 2, Name = "IVYSAUR", Types = ["FLYING"], BaseStatTotal = 405,
                Hp = 80, Attack = 62, Defence = 80, SpecialAttack = 60, SpecialDefence = 63, Speed = 60,
                Abilities = ["EFFECT SPORE", "SYNCHRONIZE"], HeldItems = ["LEPPA BERRY(common)"]
            },
            new()
            {
                Number = 3, Name = "VENUSAUR", Types = ["BUG", "WATER"], BaseStatTotal = 525,
                Hp = 80, Attack = 82, Defence = 100, SpecialAttack = 83, SpecialDefence = 100, Speed = 80,
                Abilities = ["ARENA TRAP", "STICKY HOLD"], HeldItems = ["ORAN BERRY(100%)", "LIGHT BALL(rare)"]
            }
        };

        // Act
        var result = UprLogParser.ParseLog(log).PokemonStatistics;

        // Assert
        Assert.Equivalent(expectedPokemon, result);
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

        var expectedMovesets = new List<LogMoveset>
        {
            new()
            {
                Number = 019, Name = "RATTATA", Evolution = "RATICATE",
                Hp = 72, Attack = 35, Defence = 30, SpecialAttack = 35, SpecialDefence = 25, Speed = 56,
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
                        Name = "SKULL BASH", Type = "NORMAL", Category = "PHYSICAL",
                        Power = 100, Pp = 15, Accuracy = 100, Stab = true
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
                Number = 020, Name = "RATICATE", Evolution = "(no evolution)",
                Hp = 60, Attack = 81, Defence = 97, SpecialAttack = 55, SpecialDefence = 70, Speed = 50,
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
        Assert.Equivalent(expectedMovesets, result);
    }

    [Fact]
    public void ParseLog_ParsesTmMoves()
    {
        // Arrange
        var log = LogTestData.BuildTestLog(LogTestData.LogTmMovesSection);

        var expectedTms = new List<LogTmHmMove>
        {
            new() { Number = "TM01", Move = "HYPER BEAM" },
            new() { Number = "TM02", Move = "BIDE" },
            new() { Number = "TM03", Move = "THUNDER WAVE" },
            new() { Number = "TM04", Move = "MUD-SLAP" },
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
            new() { OldMove = "MEGA PUNCH", NewMove = "DESTINY BOND" },
            new() { OldMove = "SWORDS DANCE", NewMove = "SLUDGE" },
            new() { OldMove = "MUD-SLAP", NewMove = "ENDURE" },
            new() { OldMove = "BODY SLAM", NewMove = "MUD-SLAP" },
        };

        // Act
        var result = UprLogParser.ParseLog(log).TutorMoves;

        // Assert
        Assert.Equivalent(expectedTutorMoves, result);
    }
}