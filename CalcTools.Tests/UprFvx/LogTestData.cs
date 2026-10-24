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
         29|NIDORAN♀  |WATER        | 275|  55|  47|  52|  40|  40|  41|POISON POINT|-------     |
         32|NIDORAN♂  |WATER        | 273|  46|  57|  40|  40|  40|  50|POISON POINT|-------     |
         83|FARFETCH’D|ELECTRIC/FIRE| 318|  65|  45|  65|  45|  49|  49|OVERGROW    |-------     |
        122|MR. MIME  |FLYING       | 405|  80|  62|  80|  60|  63|  60|EFFECT SPORE|SYNCHRONIZE |KING’S ROCK(common)
        233|PORYGON2  |FIRE         | 515|  85|  80|  90| 105|  95|  60|TRACE       |-------     |
        250|HO-OH     |BUG/WATER    | 525|  80|  82| 100|  83| 100|  80|ARENA TRAP  |STICKY HOLD |ORAN BERRY(100%), LIGHT BALL(rare)
        """;

    public const string LogMoveDataSectionWithoutOldName =
        """
        ( Move Data {MVDT} )

        NUM|NAME        |TYPE    |POWER|ACC.|PP
        1  |POUND       |GHOST   |50   |95% |15
        2  |KARATE CHOP |WATER   |85   |100%|25
        3  |CURSE       |???     |0    |0%  |15
        176|CONVERSION 2|FIGHTING|0    |100%|5 
        """;

    public const string LogMoveDataSectionWithOldName =
        """
        ( Move Data {MVDT} )

        NUM|OLD NAME    |NAME        |TYPE    |POWER|ACC.|PP
        1  |POUND       |CORE ASSAULT|NORMAL  |40   |100%|35
        2  |KARATE CHOP |ARMBAR RUSH |FIGHTING|50   |100%|25
        3  |CURSE       |ATTACK AURA |???     |0    |0%  |10
        176|CONVERSION 2|GLASS GLARE |GROUND  |0    |90% |40
        """;

    public const string LogPokemonMovesetsSection =
        """
        ( Pokemon Movesets {PKMV} )

        029 NIDORAN♀ -> NIDORINA
          HP| ATK| DEF|SATK|SDEF| SPD 
          55|  47|  52|  40|  40|  41 
        Level 1 : ACID        | POISON   | PHYSICAL | POW= 40 | PP=30 | ACC=100% (STAB)

        032 NIDORAN♂ -> NIDORINO
          HP| ATK| DEF|SATK|SDEF| SPD 
          46|  57|  40|  40|  40|  50 
        Level 1 : DRAGONBREATH| DRAGON   | SPECIAL  | POW= 60 | PP=20 | ACC=100%
        Level 20: GUILLOTINE  | NORMAL   | PHYSICAL | POW=  1 | PP= 5 | ACC= 30% (STAB)
        Egg Moves:
         - CONVERSION 2| NORMAL   | STATUS | POW= -- | PP=15 | ACC=100% (STAB)
         - MUD-SLAP    | GROUND   | PHYSICAL | POW= 20 | PP=10 | ACC=100%

        083 FARFETCH’D ->  (no evolution)
          HP| ATK| DEF|SATK|SDEF| SPD 
          52|  65|  55|  58|  62|  60 
        Level 1 : AERIAL ACE  | FLYING   | PHYSICAL | POW= 60 | PP=20 | ACC=  0% (STAB)

        122 MR. MIME ->  (no evolution)
          HP| ATK| DEF|SATK|SDEF| SPD 
          40|  45|  65| 100| 120|  90 
        Level 48: CURSE       | null     | STATUS   | POW= -- | PP=10 | ACC=  0%

        233 PORYGON2 ->  (no evolution)
          HP| ATK| DEF|SATK|SDEF| SPD 
          85|  80|  90| 105|  95|  60 
        Level 1 : SPIKE CANNON| NORMAL   | PHYSICAL | POW= 20 | PP=15 | ACC=100% (STAB)

        250 HO-OH ->  (no evolution)
          HP| ATK| DEF|SATK|SDEF| SPD 
         106| 130|  90| 110| 154|  90 
        Level 1 : ICE PUNCH   | ICE      | SPECIAL  | POW= 75 | PP=15 | ACC=100%
        """;

    public const string LogTmMovesSection =
        """
        ( TM Moves {TMMV} )

        TM01 HYPER BEAM
        TM02 BIDE
        TM03 CONVERSION 2
        TM04 MUD-SLAP
        """;

    public const string LogTutorMovesSection =
        """
        ( Move Tutor Moves {MTMV} )

        MEGA PUNCH -> CONVERSION 2
        SWORDS DANCE -> SLUDGE    
        MUD-SLAP  -> ENDURE    
        CONVERSION 2  -> MUD-SLAP   
        """;

    public const string LogTmHmCompatibilitySection =
        """
        ( TM/HM Compatibility {TMCB} )

        --By Species:--
        #029 NIDORAN♀
        	TM45 HAIL, TM48 CONVERSION 2, HM02 FLY,
        	HM07 WATERFALL
        #032 NIDORAN♂
            TM01 HYPER BEAM, TM05 STOMP
        #083 FARFETCH’D
            TM01 HYPER BEAM, TM02 BIDE
        #122 MR. MIME
            TM02 BIDE
        #233 PORYGON2
            TM01 HYPER BEAM, TM02 BIDE
        #250 HO-OH
            TM45 HAIL, TM50 MUD-SLAP, 
        	HM01 CUT

        --By TM/HM:--
        TM01 MUD-SLAP
            #029 NIDORAN♀, #032 NIDORAN♂,
            #083 FARFETCH’D
        TM02 CONVERSION 2
            #122 MR. MIME, #233 PORYGON2,
        HM08 DIVE
            #250 HO-OH
        """;

    public const string LogMoveTutorCompatibilitySection =
        """
        ( Move Tutor Compatibility {MTCB} )

        --By Species:--
        #029 NIDORAN♀
        	CONVERSION 2, SLUDGE,
        	MUD-SLAP
        #032 NIDORAN♂
            DESTINY BOND, SLUDGE
        #083 FARFETCH’D
            HYPER BEAM, BIDE
        #122 MR. MIME
            BIDE
        #233 PORYGON2
            HYPER BEAM, BIDE
        #250 HO-OH
            SLUDGE, FOLLOW ME, KNOCK OFF

        --By Tutor Move:--
        HYPER BEAM
            #029 NIDORAN♀, #032 NIDORAN♂,
            #083 FARFETCH’D
        CONVERSION 2
            #122 MR. MIME, #233 PORYGON2,
        MUD-SLAP
            #250 HO-OH
        """;

    public const string LogTrainerPokemonSection =
        """
        ( Trainer Pokemon {TRPK} )

        #1 (HIKER SAWYER)@310058 - FARFETCH’D Lv21 (Battle Style: Single)
        #9 ([PK][MN] BREEDER GABRIELLE)@310198 - NIDORAN♂ Lv46, NIDORAN♀ Lv48, MR. MIME Lv46, PORYGON2 Lv48, HO-OH@KING’S ROCK Lv49

        #34 (AQUA LEADER ARCHIE)@310580
        MIGHTYENA Lv41, Ability: INTIMIDATE - HYPER BEAM, MUD-SLAP, CONVERSION 2, DOUBLE TEAM
        KINGDRA@CHESTO BERRY Lv46, Ability: SWIFT SWIM - HYDRO PUMP, FRUSTRATION, SLEEP TALK, ENDURE
        (Battle Style: Double)
        """;
}