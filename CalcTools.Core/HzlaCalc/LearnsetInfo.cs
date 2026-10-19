using System.Text.Json.Serialization;

namespace CalcTools.Core.HzlaCalc;

public record LearnsetInfo
{
    [JsonPropertyName("tms")] public List<string> TMs { get; set; } = [];
    [JsonPropertyName("learnset")] public List<LevelUpMove> Learnsets { get; set; } = [];
}