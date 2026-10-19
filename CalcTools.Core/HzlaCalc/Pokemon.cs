using System.Text.Json.Serialization;

namespace CalcTools.Core.HzlaCalc;

public record Pokemon
{
    [JsonPropertyName("bs")] public Stats BaseStats { get; set; } = new();
    [JsonPropertyName("learnset_info")] public LearnsetInfo LearnsetInfo { get; set; } = new();
    [JsonPropertyName("types")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public List<PokemonType>? Types { get; set; }
}