using System.Text.Json.Serialization;

namespace CalcTools.Core.HzlaCalc;

public record CalcLearnsetInfo
{
    [JsonPropertyName("tms")] public List<string> TMs { get; set; } = [];
    [JsonPropertyName("learnset")] public List<CalcLevelUpMove> Learnsets { get; set; } = [];
}