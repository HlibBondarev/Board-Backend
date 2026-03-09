using System.Text.Json;
using System.Text.Json.Serialization;

namespace Board.Common.Extensions;

public static class JsonSerializerOptionsExtensions
{
    public static JsonSerializerOptions GetDefault(this JsonSerializerOptions jsonSerializerOptions)
    {
        ArgumentNullException.ThrowIfNull(jsonSerializerOptions);

        jsonSerializerOptions.PropertyNameCaseInsensitive = true;
        jsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
        jsonSerializerOptions.NumberHandling = JsonNumberHandling.AllowReadingFromString;

        return jsonSerializerOptions;
    }
}

