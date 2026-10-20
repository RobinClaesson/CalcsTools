namespace CalcTools.Core.UprFvx;

public record LogTutorCompatibility
{
    public List<LogTutorCompatibilityBySpecies> BySpecies { get; init; } = [];
    public List<LogTutorCompatibilityByTutorMove> ByTutorMove { get; init; } = [];
}