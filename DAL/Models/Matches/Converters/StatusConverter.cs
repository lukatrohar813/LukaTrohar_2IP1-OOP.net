using DAL.Models.Matches.Enums;
using Newtonsoft.Json;

namespace DAL.Models.Matches.Converters;

internal class StatusConverter : JsonConverter
{
    public override bool CanConvert(Type t) => t == typeof(Status) || t == typeof(Status?);

    public override object? ReadJson(JsonReader reader, Type t, object existingValue, JsonSerializer serializer)
    {
        if (reader.TokenType == JsonToken.Null) return null;
        var value = serializer.Deserialize<string>(reader);
        if (value == "completed")
        {
            return Status.Completed;
        }
        throw new Exception("Cannot unmarshal type Status");
    }

    public override void WriteJson(JsonWriter writer, object? untypedValue, JsonSerializer serializer)
    {
        if (untypedValue == null)
        {
            serializer.Serialize(writer, null);
            return;
        }
        var value = (Status)untypedValue;
        if (value != Status.Completed)
        {
            throw new Exception("Cannot marshal type Status");
        }

        serializer.Serialize(writer, "completed");
    }

    public static readonly StatusConverter Singleton = new StatusConverter();
}