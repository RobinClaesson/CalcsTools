namespace CalcTools.Core.UprFvx;

public record LogTutorCompatibilityByTutorMove
{
    public required string Move { get; init; }
    public required List<LogPokemonSpecies> Species { get; init; }
}