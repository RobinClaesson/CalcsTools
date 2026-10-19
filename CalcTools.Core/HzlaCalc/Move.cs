using System.Text.Json.Serialization;

namespace CalcTools.Core.HzlaCalc;

public record Move
{
    [JsonPropertyName("type")] public required PokemonType Type { get; set; }
    [JsonPropertyName("basePower")] public required int BasePower { get; set; }
}