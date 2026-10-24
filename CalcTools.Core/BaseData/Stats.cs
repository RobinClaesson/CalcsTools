namespace CalcTools.Core.BaseData;

public record Stats
{
    public int Hp { get; set; }
    public int Attack { get; set; }
    public int Defence { get; set; }
    public int SpecialAttack { get; set; }
    public int SpecialDefence { get; set; }
    public int Speed { get; set; }
}