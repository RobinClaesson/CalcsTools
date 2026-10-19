using System.Text.Json.Serialization;

namespace CalcTools.Core.HzlaCalc;

public record Stats
{
    [JsonPropertyName("hp")] public int Hp { get; set; }
    [JsonPropertyName("at")] public int Attack { get; set; }
    [JsonPropertyName("df")] public int Defence { get; set; }
    [JsonPropertyName("sa")] public int SpecialAttack { get; set; }
    [JsonPropertyName("sd")] public int SpecialDefence { get; set; }
    [JsonPropertyName("sp")] public int Speed { get; set; }
}