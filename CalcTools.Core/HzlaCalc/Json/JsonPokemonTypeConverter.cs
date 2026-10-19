using System.Text.Json;
using System.Text.Json.Serialization;

namespace CalcTools.Core.HzlaCalc.Json;

public class JsonPokemonTypeConverter : JsonConverter<PokemonType>
{
    public override PokemonType Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (reader.TokenType != JsonTokenType.String)
        {
            throw new JsonException("Expected a string for PokemonType.");
        }

        var logType = reader.GetString()!;

        return logType switch
        {
            "???" or "null" => PokemonType.Unkown,
            _ => Enum.Parse<PokemonType>(logType, ignoreCase: true)
        };
    }

    public override void Write(Utf8JsonWriter writer, PokemonType value, JsonSerializerOptions options)
    {
        writer.WriteStringValue(value == PokemonType.Unkown ? "???" : value.ToString());
    }
}