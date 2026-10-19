namespace CalcTools.Core.UprFvx;

public record RandomizationOverview
{
    public bool PokemonBaseStatistics { get; init; }
    public bool PokemonTypes { get; init; }
    public bool PokemonAbilities { get; init; }
    public bool PokemonEvolutions { get; init; }
    public bool StarterPokemon { get; init; }
    public bool StaticPokemon { get; init; }
    public bool InGameTrades { get; init; }
    public bool MoveData { get; init; }
    public bool PokemonMovesets { get; init; }
    public bool TrainerPokemon { get; init; }
    public bool TrainerMovesets { get; init; }
    public bool TrainerNames { get; init; }
    public bool WildPokemon { get; init; }
    public bool TmMoves { get; init; }
    public bool TmHmCompatibility { get; init; }
    public bool MoveTutorMoves { get; init; }
    public bool MoveTutorCompatibility { get; init; }
    public bool FieldItems { get; init; }
    public bool ShopItems { get; init; }
    public bool PickupItems { get; init; }
    public bool TypeEffectiveness { get; init; }
    public bool PokemonPalettes { get; init; }
    public required MiscTweaks MiscTweaks { get; init; }
    public string IntroPokemon { get; init; } = string.Empty;
}