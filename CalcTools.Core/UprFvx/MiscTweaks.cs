namespace CalcTools.Core.UprFvx;

public record MiscTweaks
{
    public bool FastestText { get; init; }
    public bool RunningShoesIndoors { get; init; }
    public bool RandomizePcPotion { get; init; }
    public bool GiveNationalDexAtStart { get; init; }
    public bool FastEggHatching { get; init; }
    public bool LowerCasePokemonNames { get; init; }
    public bool RandomizeCatchingTutorial { get; init; }
    public bool BanLuckyEgg { get; init; }
    public bool RunWithoutRunningShoes { get; init; }
    public bool ReusableTms { get; init; }
    public bool ForgettableHms { get; init; }
    public bool NoEVsFromPokemon { get; init; }
}