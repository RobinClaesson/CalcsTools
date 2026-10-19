namespace CalcTools.Core.UprFvx;

public record LogSection
{
    public required string Identifier { get; init; }
    public required string Title { get; init; }
    public required string Content { get; init; }
}