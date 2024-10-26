namespace System.Text.Json;

using System.Text.Encodings.Web;
using System.Text.Json.Serialization;

public static class JsonSerializerHelper
{
    public static JsonSerializerOptions ToReadonlyJson { get; }

    // see: https://www.cnblogs.com/RainFate/p/15720684.html#_label3_1
    static JsonSerializerHelper()
    {
        ToReadonlyJson = new JsonSerializerOptions()
        {
            WriteIndented = true,
            Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping,
            Converters = { new DateTimeConverter(), new DateTimeOffsetConverter(), },
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
        };
    }
}
