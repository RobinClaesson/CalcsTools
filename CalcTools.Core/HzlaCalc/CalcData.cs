using System.Text.Json.Serialization;

namespace CalcTools.Core.HzlaCalc;

public record CalcData
{
    [JsonPropertyName("poks")] public Dictionary<string, CalcPokemon> Pokemons { get; set; } = [];
    [JsonPropertyName("moves")] public Dictionary<string, CalcMove> Moves { get; set; } = [];

    [JsonPropertyName("formatted_sets")]
    public Dictionary<string, Dictionary<string, CalcSet>> FormattedSets { get; set; } = [];
}