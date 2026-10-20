namespace CalcTools.Core.UprFvx;

public record LogMove()
{
    public int Number { get; init; }
    public string? OldName { get; init; }
    public string Name { get; init; } = string.Empty;
    public string Type { get; init; } = string.Empty;
    public int Power { get; init; }
    public int Accuracy { get; init; }
    public int Pp { get; init; }
}