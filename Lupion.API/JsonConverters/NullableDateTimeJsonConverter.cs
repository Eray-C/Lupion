using System.Text.Json;
using System.Text.Json.Serialization;

namespace Lupion.API.JsonConverters;

public class NullableDateTimeJsonConverter : JsonConverter<DateTime?>
{
    public override DateTime? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (reader.TokenType == JsonTokenType.Null)
            return null;
        if (reader.TokenType == JsonTokenType.String)
        {
            var s = reader.GetString();
            if (string.IsNullOrWhiteSpace(s))
                return null;
            if (DateTime.TryParse(s, null, System.Globalization.DateTimeStyles.RoundtripKind, out var dt))
                return dt;
            return null;
        }
        if (reader.TokenType == JsonTokenType.Number && reader.TryGetInt64(out var l))
            return DateTimeOffset.FromUnixTimeMilliseconds(l).UtcDateTime;
        return null;
    }

    public override void Write(Utf8JsonWriter writer, DateTime? value, JsonSerializerOptions options)
    {
        if (value.HasValue)
            writer.WriteStringValue(value.Value);
        else
            writer.WriteNullValue();
    }
}
