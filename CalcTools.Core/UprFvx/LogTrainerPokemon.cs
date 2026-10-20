namespace CalcTools.Core.UprFvx;

public record LogTrainerPokemon
{
    public required string Pokemon { get; init; }
    public required int Level { get; init; }
    public string? Item { get; init; }
    public string? Ability { get; init; }
    public List<string> Moves { get; init; } = [];
}