namespace CalcTools.Core.UprFvx;

public record LogMovesetMove
{
    public string Name { get; init; } = string.Empty;
    public string Type { get; init; } = string.Empty;
    public string Category { get; init; } = string.Empty;
    public int Power { get; init; }
    public int Pp { get; init; }
    public int Accuracy { get; init; }
    public bool Stab { get; init; }

    private static int ParseNumber(string field) =>
        int.TryParse(field.Split('=').Last(), out var result) ? result : 0;
}