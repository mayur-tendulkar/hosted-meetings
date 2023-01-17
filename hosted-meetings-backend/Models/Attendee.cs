using System.Text.Json.Serialization;

namespace hosted_meetings_backend.Models
{
    public class Attendee
    {
        [JsonPropertyName("name")]
        public string? Name { get; set; }

        [JsonPropertyName("email")]
        public string? Email { get; set; }
        [JsonPropertyName("client_specific_id")]
        public string? ClientSpecificId { get; set; }
        [JsonPropertyName("preset_name")]
        public string? PresetName { get; set; }
        [JsonPropertyName("picture")]
        public string? PictureUrl { get; set; }
    }
}
