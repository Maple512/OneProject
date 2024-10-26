namespace System.Text.Json.Serialization;

public class DateTimeOffsetConverter : JsonConverter<DateTimeOffset>
{
    public override DateTimeOffset Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        var str = reader.GetString();

        if(string.IsNullOrWhiteSpace(str))
        {
            return DateTimeOffset.MinValue;
        }

        return DateTimeOffset.Parse(str);
    }

    public override void Write(Utf8JsonWriter writer, DateTimeOffset value, JsonSerializerOptions options)
    {
        writer.WriteStringValue(value.LocalDateTime.ToString("yyyy-MM-dd HH:mm:ss"));
    }
}
