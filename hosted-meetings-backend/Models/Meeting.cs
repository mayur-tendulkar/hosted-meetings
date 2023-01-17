using System.Text.Json.Serialization;

namespace hosted_meetings_backend.Models
{
    public class Meeting
    {
        [JsonPropertyName("title")]
        public string? Title { get; set; }

    }
}
