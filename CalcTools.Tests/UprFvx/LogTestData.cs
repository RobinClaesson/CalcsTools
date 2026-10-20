using System.Text;
using CalcTools.Core.Extensions;
using PokemonType = CalcTools.Core.HzlaCalc.PokemonType;

namespace CalcTools.Tests.UprFvx;

public static class LogTestData
{
    public static readonly List<TheoryDataRow<string, PokemonType>> SpecialTypeStrings =
    [
        new("???", PokemonType.Unkown),
        new("null", PokemonType.Unkown)
    ];

    public static IEnumerable<TheoryDataRow<string, PokemonType>> PokemonTypes => Enum.GetValues<PokemonType>()
        .Select(pt => (Type: pt, String: pt.ToString()))
        .SelectMany(pt => new List<TheoryDataRow<string, PokemonType>>
        {
            new(pt.String.ToUpper(), pt.Type),
            new(pt.String.ToLower(), pt.Type),
            new(pt.String.ToTitleCase(), pt.Type),
        })
        .Concat(SpecialTypeStrings);


    public static string BuildTestLog(params List<string> logSections)
    {
        var sb = new StringBuilder(LogHeader)
            .AppendLine(LogInformation).AppendLine(LogDivider)
            .AppendLine(LogTableOfContentsSection).AppendLine(LogDivider)
            .AppendLine(LogOverviewSection);

        logSections.ForEach(section => sb
            .AppendLine(LogDivider)
            .AppendLine(section));

        return sb.ToString();
    }

    public const string LogHeader =
        """
        ==========================================================
        |   _   _        _                           _           |
        |  | | | | _ _  (_)__ __ ___  _ _  ___ __ _ | |          |
        |  | |_| || ' \ | |\ V // -_)| '_|(_-</ _` || |          |
        |   \___/ |_||_||_| \_/ \___||_|  /__/\__,_||_|          |
        |   ___       _     __                                   |
        |  | _ \ ___ | |__ /_/  _ __   ___  _ _                  |
        |  |  _// _ \| / // -_)| '  \ / _ \| ' \                 |
        |  |_|  \___/|_\_\\___||_|_|_|\___/|_||_|                |
        |   ___                 _              _                 |
        |  | _ \ __ _  _ _   __| | ___  _ __  (_) ___ ___  _ _   |
        |  |   // _` || ' \ / _` |/ _ \| '  \ | ||_ // -_)| '_|  |
        |  |_|_\\__,_||_||_|\__,_|\___/|_|_|_||_|/__|\___||_|    |
        |   ___ __   ____  __                                    |
        |  | __|\ \ / /\ \/ /                                    |
        |  | _|  \ V /  >  <                                     |
        |  |_|    \_/  /_/\_\  v1.6.1                            |
        |                                                        |
        ==========================================================
        """;

    public const string LogDivider =
        """

        ==========================================================

        """;

    public const string LogInformation =
        """

        [ Log for Randomized Game ]

        Base game: Pokemon Emerald (U)
        Randomizer version: FVX 1.6.1
        Random seed: 161080680643607
        Settings string: 427AAQEAQT8AP8AAgECAAHkCQARAQEUAAAUAEAEAAEA/////wAAAADkBOQBAAgJ5AYEAOQAAgABAAEBAAAAAAAJAAABKAEAE1Bva2Vtb24gRW1lcmFsZCAoVSkrhZUfAAAAAA==

        If you are having problems using the Universal Pokémon Randomizer FVX,

        Wiki link: https://upr-fvx.github.io/universal-pokemon-randomizer-fvx/wikipages
        GitHub issues page link: https://github.com/upr-fvx/universal-pokemon-randomizer-fvx/issues
        """;

    public const string LogTableOfContentsSection =
        """
        ( Table of Contents {TABL} )

        Search (ctrl-f) for the strings in curly brackets {}
        to find the below sections quickly.

         Table of Contents--------------------------------{TABL}
         Overview of Randomization------------------------{OVRD}

         Randomization Statistics-------------------------{STAT}
         Randomization/ROM Diagnostics--------------------{DIAG}
        """;

    public const string LogOverviewSection =
        """
        ( Overview of Randomization {OVRD} )

        Pokemon Base Statistics: Randomized/Changed
        Pokemon Types: Randomized/Changed
        Pokemon Abilities: Randomized/Changed
        Pokemon Evolutions: Unchanged
        Starter Pokemon: Randomized/Changed
        Static Pokemon: Unchanged
        In-Game Trades: Unchanged
        Move Data: Randomized/Changed
        Pokemon Movesets: Unchanged
        Trainer Pokemon: Unchanged
        Trainer Movesets: Unchanged
        Trainer Names: Unchanged
        Wild Pokemon: Randomized/Changed
        TM Moves: Unchanged
        TM/HM Compatibility: Unchanged
        Move Tutor Moves: Unchanged
        Move Tutor Compatibility: Unchanged
        Field Items: Unchanged
        Shop Items: Unchanged
        Pickup Items: Unchanged
        Type Effectiveness: Randomized/Changed
        Pokemon Palettes: Unchanged

        The following Misc. Tweaks were applied:
        Fastest Text
        Running Shoes Indoors

        Intro Pokemon: RELICANTH
        """;

    public const string LogPokemonStatisticsSection =
        """
        ( Pokemon Base Statistics / Types / Abilities {PKST} )

        NUM|NAME      |TYPE         | BST|  HP| ATK| DEF|SATK|SDEF| SPD|ABILITY 1   |ABILITY 2   |HELD ITEM
          1|BULBASAUR |ELECTRIC/FIRE| 318|  65|  45|  65|  45|  49|  49|OVERGROW    |-------     |
          2|IVYSAUR   |FLYING       | 405|  80|  62|  80|  60|  63|  60|EFFECT SPORE|SYNCHRONIZE |LEPPA BERRY(common)
          3|VENUSAUR  |BUG/WATER    | 525|  80|  82| 100|  83| 100|  80|ARENA TRAP  |STICKY HOLD |ORAN BERRY(100%), LIGHT BALL(rare)
        """;

    public const string LogMoveDataWithoutOldName =
        """
        ( Move Data {MVDT} )

        NUM|NAME        |TYPE    |POWER|ACC.|PP
        1  |POUND       |GHOST   |50   |95% |15
        2  |KARATE CHOP |WATER   |85   |100%|25
        3  |CURSE       |???     |0    |0%  |15
        """;

    public const string LogMoveDataWithOldName =
        """
        ( Move Data {MVDT} )

        NUM|OLD NAME    |NAME        |TYPE    |POWER|ACC.|PP
        1  |POUND       |CORE ASSAULT|NORMAL  |40   |100%|35
        2  |KARATE CHOP |ARMBAR RUSH |FIGHTING|50   |100%|25
        3  |CURSE       |ATTACK AURA |???     |0    |0%  |10
        """;

    public const string LogPokemonMovesets =
        """
        ( Pokemon Movesets {PKMV} )

        019 RATTATA -> RATICATE
          HP| ATK| DEF|SATK|SDEF| SPD 
          72|  35|  30|  35|  25|  56 
        Level 1 : DRAGONBREATH| DRAGON   | SPECIAL  | POW= 60 | PP=20 | ACC=100%
        Level 20: GUILLOTINE  | NORMAL   | PHYSICAL | POW=  1 | PP= 5 | ACC= 30% (STAB)
        Egg Moves:
         - SKULL BASH  | NORMAL   | PHYSICAL | POW=100 | PP=15 | ACC=100% (STAB)
         - MUD-SLAP    | GROUND   | PHYSICAL | POW= 20 | PP=10 | ACC=100%

        020 RATICATE ->  (no evolution)
          HP| ATK| DEF|SATK|SDEF| SPD 
          60|  81|  97|  55|  70|  50 
        Level 48: CURSE       | null     | STATUS   | POW= -- | PP=10 | ACC=  0%
        """;

    public const string LogTmMoves =
        """
        ( TM Moves {TMMV} )
        
        TM01 HYPER BEAM
        TM02 BIDE
        TM03 THUNDER WAVE
        TM04 LICK
        """;
}