namespace CalcTools.Core.UprFvx;

public record LogPokemon()
{
    public int Num { get; init; }
    public string Name { get; init; } = string.Empty;
    public string[] Types { get; init; } = [];
    public int BaseStatTotal { get; init; }
    public int Hp { get; init; }
    public int Attack { get; init; }
    public int Defence { get; init; }
    public int SpecialAttack { get; init; }
    public int SpecialDefence { get; init; }
    public int Speed { get; init; }
    public string[] Abilities { get; init; } = [];
    public string[] HeldItems { get; init; } = [];
}