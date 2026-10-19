namespace CalcTools.Core.UprFvx;

public record LogMoveset
{
    public int Num { get; init; }
    public string Name { get; init; } = string.Empty;
    public string Evolution { get; init; } = string.Empty;
    public int Hp { get; init; }
    public int Attack { get; init; }
    public int Defence { get; init; }
    public int SpecialAttack { get; init; }
    public int SpecialDefence { get; init; }
    public int Speed { get; init; }
    public List<LogMovesetLevelUpMove> LevelUpMoves { get; init; } = [];
    public List<LogMovesetMove> EggMoves { get; init; } = [];
}