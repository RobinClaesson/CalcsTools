using System.Text.Json;
using System.Text.Json.Serialization;

namespace CalcTools.Core.HzlaCalc.Json;

public class JsonCalcLevelUpMoveConverter : JsonConverter<CalcLevelUpMove>
{
    public override CalcLevelUpMove Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (reader.TokenType != JsonTokenType.StartArray)
            throw new JsonException("Expected an array.");

        reader.Read();
        var level = reader.GetInt32();

        reader.Read();
        var move = reader.GetString() ?? throw new JsonException("Move cannot be null.");

        reader.Read();
        if (reader.TokenType != JsonTokenType.EndArray)
            throw new JsonException("Expected exactly two elements.");

        return new CalcLevelUpMove
        {
            Level = level,
            Move = move
        };
    }

    public override void Write(Utf8JsonWriter writer, CalcLevelUpMove value, JsonSerializerOptions options)
    {
        writer.WriteStartArray();
        writer.WriteNumberValue(value.Level);
        writer.WriteStringValue(value.Move);
        writer.WriteEndArray();
    }
}