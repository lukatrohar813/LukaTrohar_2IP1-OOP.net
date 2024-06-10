using DAL.Models.Matches.Converters;
using DAL.Models.Matches.Enums;
using Newtonsoft.Json;

namespace DAL.Models.Matches;

public class Match
{
    [JsonProperty("venue")]
    public string? Venue { get; set; }

    [JsonProperty("location")]
    public string? Location { get; set; }

    [JsonProperty("status")]
    public Status Status { get; set; }

    [JsonProperty("time")]
    [JsonConverter(typeof(TimeConverter))]
    public Time Time { get; set; }

    [JsonProperty("fifa_id")]
    [JsonConverter(typeof(ParseStringConverter))]
    public long FifaId { get; set; }

    [JsonProperty("weather")]
    public Weather? Weather { get; set; }

    [JsonProperty("attendance")]
    [JsonConverter(typeof(ParseStringConverter))]
    public long Attendance { get; set; }

    [JsonProperty("officials")]
    public List<string>? Officials { get; set; }

    [JsonProperty("stage_name")]
    [JsonConverter(typeof(StageNameConverter))]
    public StageName StageName { get; set; }

    [JsonProperty("home_team_country")]
    public string? HomeTeamCountry { get; set; }

    [JsonProperty("away_team_country")]
    public string? AwayTeamCountry { get; set; }

    [JsonProperty("datetime")]
    public DateTimeOffset Datetime { get; set; }

    [JsonProperty("winner")]
    public string? Winner { get; set; }

    [JsonProperty("winner_code")]
    public string? WinnerCode { get; set; }

    [JsonProperty("home_team")]
    public MatchTeam HomeTeam { get; set; }

    [JsonProperty("away_team")]
    public MatchTeam AwayTeam { get; set; }

    [JsonProperty("home_team_events")]
    public List<TeamEvent>? HomeTeamEvents { get; set; }

    [JsonProperty("away_team_events")]
    public List<TeamEvent>? AwayTeamEvents { get; set; }

    [JsonProperty("home_team_statistics")]
    public TeamStatistics? HomeTeamStatistics { get; set; }

    [JsonProperty("away_team_statistics")]
    public TeamStatistics? AwayTeamStatistics { get; set; }

    [JsonProperty("last_event_update_at")]
    public DateTimeOffset LastEventUpdateAt { get; set; }

    [JsonProperty("last_score_update_at")]
    public DateTimeOffset? LastScoreUpdateAt { get; set; }
    public override string ToString()
    {
        return $"{HomeTeam} : {AwayTeam} - {Venue} {Location}";
    }

  
    
}