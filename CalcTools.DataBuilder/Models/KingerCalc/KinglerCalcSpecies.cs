using System.Text.Json.Serialization;
using CalcTools.Core.BaseData;

namespace CalcTools.DataBuilder.Models.KingerCalc;

public record KinglerCalcSpecies
{
    [JsonPropertyName("types")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public List<PokemonType>? Types { get; init; }

    [JsonPropertyName("bs")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public KinglerCalcStats? BaseStats { get; init; }

    [JsonPropertyName("weightkg")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public double? WeightKg { get; init; }

    [JsonPropertyName("nfe")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public bool? NotFullyEvolved { get; init; }

    [JsonPropertyName("gender")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? Gender { get; init; }

    [JsonPropertyName("abilities")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public Dictionary<string, string>? Abilities { get; init; }

    [JsonPropertyName("otherFormes")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public List<string>? OtherFormes { get; init; }

    [JsonPropertyName("baseSpecies")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? BaseSpecies { get; init; }
}
