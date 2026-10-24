using System.Text.Json.Serialization;
using CalcTools.Core.BaseData;

namespace CalcTools.Core.HzlaCalc;

public record CalcMove
{
    [JsonPropertyName("type")] public required PokemonType Type { get; set; }
    [JsonPropertyName("basePower")] public required int BasePower { get; set; }
}