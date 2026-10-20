namespace CalcTools.Core.UprFvx;

public record LogTmHmCompatabilityByTmHm
{
    public required string Number { get; init; }
    public required string Move {get; init; }
    public required List<LogPokemonSpecies> Species { get; init; } 
}