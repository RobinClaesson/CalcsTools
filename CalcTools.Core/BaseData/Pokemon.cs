namespace CalcTools.Core.BaseData;

public record Pokemon
{
    public required string Name { get; set; }
    public required Stats BaseStats { get; set; } 
    public required List<PokemonType> Types { get; set; }
}