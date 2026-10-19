using System.Text.Json.Serialization;
using CalcTools.Core.HzlaCalc.Json;

namespace CalcTools.Core.HzlaCalc;

[JsonConverter(typeof(JsonLevelUpMoveConverter))]
public record LevelUpMove
{
    public required int Level { get; set; }
    public required string Move { get; set; }
}