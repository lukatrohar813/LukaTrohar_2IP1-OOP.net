using DAL.Models.Matches.Converters;
using DAL.Models.Matches.Enums;
using Newtonsoft.Json;

namespace DAL.Models.Matches
{
    public class TeamEvent
    {
        [JsonProperty("id")]
        public long Id { get; set; }

        [JsonProperty("type_of_event")]
        [JsonConverter(typeof(TypeOfEventConverter))]
        public TypeOfEvent TypeOfEvent { get; set; }

        [JsonProperty("player")]
        public string Player { get; set; }

        [JsonProperty("time")]
        public string Time { get; set; }
    }
}