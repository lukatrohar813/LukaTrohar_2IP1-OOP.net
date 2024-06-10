using Newtonsoft.Json;

namespace DAL.Models.Matches
{
    public class MatchTeam
    {
        [JsonProperty("country")]
        public string? Country { get; set; }

        [JsonProperty("code")]
        public string? Code { get; set; }

        [JsonProperty("goals")]
        public long Goals { get; set; }

        [JsonProperty("penalties")]
        public long Penalties { get; set; }
        public override string ToString()
        {
            return $"{Country.ToUpper()} ({Code.ToUpper()})";
        }
    }
}
