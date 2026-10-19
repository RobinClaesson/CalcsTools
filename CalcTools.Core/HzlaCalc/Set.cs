using System.Text.Json.Serialization;

namespace CalcTools.Core.HzlaCalc;

public record Set
{
    [JsonPropertyName("evs")] public Stats Evs { get; set; } = new();
    [JsonPropertyName("ivs")] public Stats Ivs { get; set; } = new();
    [JsonPropertyName("item")] public string Item { get; set; } = string.Empty;
    [JsonPropertyName("level")] public int Level { get; set; }
    [JsonPropertyName("moves")] public List<string> Moves { get; set; } = [];
    [JsonPropertyName("nature")] public string Nature { get; set; } = string.Empty;
    [JsonPropertyName("ability")] public string Ability { get; set; } = string.Empty;
    [JsonPropertyName("sub_index")] public int SubIndex { get; set; }
    [JsonPropertyName("battle_type")] public string BattleType { get; set; } = "Singles";
}