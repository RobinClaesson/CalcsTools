using CalcTools.Core.UprFvx.Constants;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;

namespace CalcTools.Web.Components.Log;

public partial class LogFileInfo : LogComponentBase
{
    [Parameter] public EventCallback<MouseEventArgs> OnCreateCalcDataClick { get; set; }
    [Parameter] public bool ShowAllChanges { get; set; }

    private string ChangesHeaderText => ShowAllChanges ? "All Changes & Misc. Tweaks" : "Changes To Calc";

    private Dictionary<string, bool> ChangesEffectingCalc => Log is not null
        ? new Dictionary<string, bool>
        {
            { RandomizationOverviewKeys.PokemonBaseStatistics, Log.RandomizationOverview.PokemonBaseStatistics },
            { RandomizationOverviewKeys.PokemonTypes, Log.RandomizationOverview.PokemonTypes },
            { RandomizationOverviewKeys.PokemonAbilities, Log.RandomizationOverview.PokemonAbilities },
            { RandomizationOverviewKeys.MoveData, Log.RandomizationOverview.MoveData },
            { RandomizationOverviewKeys.PokemonMovesets, Log.RandomizationOverview.PokemonMovesets },
            { RandomizationOverviewKeys.TrainerPokemon, Log.RandomizationOverview.TrainerPokemon },
            { RandomizationOverviewKeys.TrainerMovesets, Log.RandomizationOverview.TrainerMovesets },
            { RandomizationOverviewKeys.TrainerNames, Log.RandomizationOverview.TrainerNames },
            { RandomizationOverviewKeys.TmMoves, Log.RandomizationOverview.TmMoves },
            { RandomizationOverviewKeys.TmHmCompatibility, Log.RandomizationOverview.TmHmCompatibility },
        }
        : [];

    private Dictionary<string, bool> Changes => Log is not null
        ? new Dictionary<string, bool>
        {
            { RandomizationOverviewKeys.PokemonBaseStatistics, Log.RandomizationOverview.PokemonBaseStatistics },
            { RandomizationOverviewKeys.PokemonTypes, Log.RandomizationOverview.PokemonTypes },
            { RandomizationOverviewKeys.PokemonAbilities, Log.RandomizationOverview.PokemonAbilities },
            { RandomizationOverviewKeys.PokemonEvolutions, Log.RandomizationOverview.PokemonEvolutions },
            { RandomizationOverviewKeys.StarterPokemon, Log.RandomizationOverview.StarterPokemon },
            { RandomizationOverviewKeys.StaticPokemon, Log.RandomizationOverview.StaticPokemon },
            { RandomizationOverviewKeys.InGameTrades, Log.RandomizationOverview.InGameTrades },
            { RandomizationOverviewKeys.MoveData, Log.RandomizationOverview.MoveData },
            { RandomizationOverviewKeys.PokemonMovesets, Log.RandomizationOverview.PokemonMovesets },
            { RandomizationOverviewKeys.TrainerPokemon, Log.RandomizationOverview.TrainerPokemon },
            { RandomizationOverviewKeys.TrainerMovesets, Log.RandomizationOverview.TrainerMovesets },
            { RandomizationOverviewKeys.TrainerNames, Log.RandomizationOverview.TrainerNames },
            { RandomizationOverviewKeys.WildPokemon, Log.RandomizationOverview.WildPokemon },
            { RandomizationOverviewKeys.TmMoves, Log.RandomizationOverview.TmMoves },
            { RandomizationOverviewKeys.TmHmCompatibility, Log.RandomizationOverview.TmHmCompatibility },
            { RandomizationOverviewKeys.MoveTutorMoves, Log.RandomizationOverview.MoveTutorMoves },
            { RandomizationOverviewKeys.MoveTutorCompatibility, Log.RandomizationOverview.MoveTutorCompatibility },
            { RandomizationOverviewKeys.FieldItems, Log.RandomizationOverview.FieldItems },
            { RandomizationOverviewKeys.ShopItems, Log.RandomizationOverview.ShopItems },
            { RandomizationOverviewKeys.PickupItems, Log.RandomizationOverview.PickupItems },
            { RandomizationOverviewKeys.TypeEffectiveness, Log.RandomizationOverview.TypeEffectiveness },
            { RandomizationOverviewKeys.PokemonPalettes, Log.RandomizationOverview.PokemonPalettes },
        }
        : [];

    private Dictionary<string, bool> MiscTweaks => Log is not null
        ? new Dictionary<string, bool>
        {
            { MiscTweakTexts.FastestText, Log.RandomizationOverview.MiscTweaks.FastestText },
            { MiscTweakTexts.RunningShoesIndoors, Log.RandomizationOverview.MiscTweaks.RunningShoesIndoors },
            { MiscTweakTexts.RandomizePcPotion, Log.RandomizationOverview.MiscTweaks.RandomizePcPotion },
            { MiscTweakTexts.GiveNationalDexAtStart, Log.RandomizationOverview.MiscTweaks.GiveNationalDexAtStart },
            { MiscTweakTexts.FastEggHatching, Log.RandomizationOverview.MiscTweaks.FastEggHatching },
            { MiscTweakTexts.LowerCasePokemonNames, Log.RandomizationOverview.MiscTweaks.LowerCasePokemonNames },
            {
                MiscTweakTexts.RandomizeCatchingTutorial, Log.RandomizationOverview.MiscTweaks.RandomizeCatchingTutorial
            },
            { MiscTweakTexts.BanLuckyEgg, Log.RandomizationOverview.MiscTweaks.BanLuckyEgg },
            { MiscTweakTexts.RunWithoutRunningShoes, Log.RandomizationOverview.MiscTweaks.RunWithoutRunningShoes },
            { MiscTweakTexts.ReusableTms, Log.RandomizationOverview.MiscTweaks.ReusableTms },
            { MiscTweakTexts.ForgettableHms, Log.RandomizationOverview.MiscTweaks.ForgettableHms },
            { MiscTweakTexts.NoEVsFromPokemon, Log.RandomizationOverview.MiscTweaks.NoEVsFromPokemon },
        }
        : [];
}