using DAL.Models.Matches.Enums;
using Newtonsoft.Json;

namespace DAL.Models.Matches.Converters
{
    internal class DescriptionConverter : JsonConverter
    {
        public override bool CanConvert(Type t)
        {
            return t == typeof(Description) || t == typeof(Description?);
        }

        public override object? ReadJson(JsonReader reader, Type t, object existingValue, JsonSerializer serializer)
        {
            if (reader.TokenType == JsonToken.Null) return null;
            var value = serializer.Deserialize<string>(reader);
            switch (value)
            {
                case "Clear Night":
                    return Description.ClearNight;
                case "Cloudy":
                    return Description.Cloudy;
                case "Partly Cloudy":
                    return Description.PartlyCloudy;
                case "Cloudy Night":
                    return Description.CloudyNight;
                case "Partly Cloudy Night":
                    return Description.PartlyCloudyNight;
                case "Sunny":
                    return Description.Sunny;
            }
            throw new Exception("Cannot un-marshal type Description");
        }

        public override void WriteJson(JsonWriter writer, object? untypedValue, JsonSerializer serializer)
        {
            if (untypedValue == null)
            {
                serializer.Serialize(writer, null);
                return;
            }
            var value = (Description)untypedValue;
            switch (value)
            {
                case Description.ClearNight:
                    serializer.Serialize(writer, "Clear Night");
                    return;
                case Description.Cloudy:
                    serializer.Serialize(writer, "Cloudy");
                    return;
                case Description.PartlyCloudy:
                    serializer.Serialize(writer, "Partly Cloudy");
                    return;
                case Description.CloudyNight:
                    serializer.Serialize(writer, "Cloudy Night");
                    return;
                case Description.PartlyCloudyNight:
                    serializer.Serialize(writer, "Partly Cloudy Night");
                    return;
                case Description.Sunny:
                    serializer.Serialize(writer, "Sunny");
                    return;
            }
            throw new Exception("Cannot marshal type Description");
        }

        public static readonly DescriptionConverter Singleton = new();
    }
}
