using System.Text.Json.Serialization;

namespace CalcTools.DataBuilder.Models.KingerCalc;

public record KinglerCalcStats
{
    [JsonPropertyName("hp")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public int? Hp { get; init; }

    [JsonPropertyName("at")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public int? Attack { get; init; }

    [JsonPropertyName("df")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public int? Defence { get; init; }

    [JsonPropertyName("sp")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public int? Speed { get; init; }
    
    [JsonPropertyName("sl")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public int? Special { get; init; }

    [JsonPropertyName("sa")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public int? SpecialAttack { get; init; }

    [JsonPropertyName("sd")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public int? SpecialDefence { get; init; }
}
