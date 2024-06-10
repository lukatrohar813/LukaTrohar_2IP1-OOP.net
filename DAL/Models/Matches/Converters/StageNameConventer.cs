using DAL.Models.Matches.Enums;
using Newtonsoft.Json;

namespace DAL.Models.Matches.Converters;

internal class StageNameConverter : JsonConverter
{
    public override bool CanConvert(Type t)
    {
        return t == typeof(StageName) || t == typeof(StageName?);
    }

    public override object? ReadJson(JsonReader reader, Type t, object existingValue, JsonSerializer serializer)
    {
        if (reader.TokenType == JsonToken.Null) return null;
        var value = serializer.Deserialize<string>(reader);
        switch (value)
        {
            case "Final":
                return StageName.Final;
            case "First stage":
                return StageName.FirstStage;
            case "First Stage":
                return StageName.FirstStage;
            case "Play-off for third place":
                return StageName.PlayOffForThirdPlace;
            case "Match for third place":
                return StageName.PlayOffForThirdPlace;
            case "Quarter-finals":
                return StageName.QuarterFinals;
            case "Quarter-final":
                return StageName.QuarterFinals;
            case "Round of 16":
                return StageName.RoundOf16;
            case "Semi-finals":
                return StageName.SemiFinals;
            case "Semi-final":
                return StageName.SemiFinals;
        }
        throw new Exception("Cannot un-marshal type StageName");
    }

    public override void WriteJson(JsonWriter writer, object? untypedValue, JsonSerializer serializer)
    {
        if (untypedValue == null)
        {
            serializer.Serialize(writer, null);
            return;
        }
        var value = (StageName)untypedValue;
        switch (value)
        {
            case StageName.Final:
                serializer.Serialize(writer, "Final");
                return;
            case StageName.FirstStage:
                serializer.Serialize(writer, "First stage");
                return;
            case StageName.PlayOffForThirdPlace:
                serializer.Serialize(writer, "Play-off for third place");
                return;
            case StageName.QuarterFinals:
                serializer.Serialize(writer, "Quarter-finals");
                return;
            case StageName.RoundOf16:
                serializer.Serialize(writer, "Round of 16");
                return;
            case StageName.SemiFinals:
                serializer.Serialize(writer, "Semi-finals");
                return;
        }
        throw new Exception("Cannot marshal type StageName");
    }

    public static readonly StageNameConverter Singleton = new StageNameConverter();
}