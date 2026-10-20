namespace CalcTools.Core.UprFvx;

public record LogTrainer
{
    public required int Number { get; init; }
    public required string Name { get; init; }
    public required string Id { get; init; }
    public required List<LogTrainerPokemon> Pokemon { get; init; }
}