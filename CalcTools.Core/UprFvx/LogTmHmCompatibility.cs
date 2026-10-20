namespace CalcTools.Core.UprFvx;

public record LogTmHmCompatibility
{
    public List<LogTmHmCompatabilityBySpecies> BySpecies { get; init; } = [];
    public List<LogTmHmCompatabilityByTmHm> ByTmHm { get; init; } = [];
}