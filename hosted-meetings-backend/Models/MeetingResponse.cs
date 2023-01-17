using System.Text.Json.Serialization;

namespace hosted_meetings_backend.Models
{
    public class MeetingResponse
    {
        [JsonPropertyName("success")]
        public bool? Success { get; set; }
        [JsonPropertyName("data")]
        public Data? Data { get; set; }
    }

    public class Data
    {
        [JsonPropertyName("id")]
        public string? MeetingId { get; set; }
        [JsonPropertyName("preferred_region")]
        public string? PreferredRegion { get; set; }
        [JsonPropertyName("record_on_start")]
        public bool? IsRecordOnStart { get; set; }
        [JsonPropertyName("created_at")]
        public DateTime? CreatedOn { get; set; }
        [JsonPropertyName("updated_at")]
        public DateTime? UpdatedOn { get; set; }
    }
}
