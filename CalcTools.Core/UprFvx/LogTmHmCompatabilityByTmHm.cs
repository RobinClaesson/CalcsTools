namespace CalcTools.Core.UprFvx;

public record LogTmHmCompatabilityByTmHm
{
    public required string Name { get; init; }
    public required int Number { get; init; }
    public required string Move {get; init; }
    public required List<LogPokemonSpecies> Species { get; init; } 
}