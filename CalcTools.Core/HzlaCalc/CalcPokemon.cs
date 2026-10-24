using System.Text.Json.Serialization;
using CalcTools.Core.BaseData;

namespace CalcTools.Core.HzlaCalc;

public record CalcPokemon
{
    [JsonPropertyName("bs")] public CalcStats BaseStats { get; set; } = new();
    [JsonPropertyName("learnset_info")] public CalcLearnsetInfo LearnsetInfo { get; set; } = new();
    [JsonPropertyName("types")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public List<PokemonType>? Types { get; set; }
}