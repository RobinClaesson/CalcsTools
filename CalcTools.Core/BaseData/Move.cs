namespace CalcTools.Core.BaseData;

public record Move
{
    public required string Name { get; set; }
    public required PokemonType Type { get; set; }
    public required MoveCategory Category { get; set; }
    public required int BasePower { get; set; }
    public required int Accuracy { get; set; }
    public required int Pp { get; set; }
}