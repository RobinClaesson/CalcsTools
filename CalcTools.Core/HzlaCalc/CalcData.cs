using System.Text.Json.Serialization;

namespace CalcTools.Core.HzlaCalc;

public record CalcData
{
    [JsonPropertyName("poks")] public Dictionary<string, Pokemon> Pokemons { get; set; } = [];
    [JsonPropertyName("moves")] public Dictionary<string, Move> Moves { get; set; } = [];

    [JsonPropertyName("formatted_sets")]
    public Dictionary<string, Dictionary<string, Set>> FormattedSets { get; set; } = [];
}