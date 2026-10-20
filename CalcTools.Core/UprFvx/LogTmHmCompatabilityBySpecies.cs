namespace CalcTools.Core.UprFvx;

public record LogTmHmCompatabilityBySpecies
{
    public required int Number { get; init; }
    public required string Pokemon { get; init; }
    public required List<LogTmHmMove> Moves { get; init; }
}