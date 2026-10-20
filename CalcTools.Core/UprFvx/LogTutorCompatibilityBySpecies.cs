namespace CalcTools.Core.UprFvx;

public record LogTutorCompatibilityBySpecies
{
    public required int Number { get; init; }
    public required string Pokemon { get; init; }
    public required List<string> Moves { get; init; }
}