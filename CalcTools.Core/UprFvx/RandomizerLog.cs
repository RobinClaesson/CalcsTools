using CalcTools.Core.UprFvx.Constants;

namespace CalcTools.Core.UprFvx;

public record RandomizerLog
{
    public required string BaseGame { get; init; }
    public required string RandomizerVersion { get; init; }
    public required ulong RandomSeed { get; init; }
    public required string SettingsString { get; init; }
    public required RandomizationOverview RandomizationOverview { get; init; }
    public List<LogPokemon> PokemonStatistics { get; init; } = [];
    public List<LogMove> MoveData { get; init; } = [];
    public List<LogMoveset> PokemonMovesets { get; init; } = [];
    public List<LogTmMove> TmMoves { get; init; } = [];
    public List<LogTutorMove> TutorMoves { get; init; } = [];
    public Dictionary<string, LogSection> Sections { get; init; } = [];
}