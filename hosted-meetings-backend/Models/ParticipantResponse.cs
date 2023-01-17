using System.Text.Json.Serialization;

namespace hosted_meetings_backend.Models
{
    public class ParticipantResponse
    {
        [JsonPropertyName("success")]
        public bool SuccessCode { get; set; }
        [JsonPropertyName("data")]
        public ParticipantData ParticipantData { get; set; }
    }

    
    public class ParticipantData
    {
        [JsonPropertyName("created_at")]
        public DateTime CreatedOn { get; set; }
        [JsonPropertyName("updated_at")]
        public DateTime UpdatedOn { get; set; }
        [JsonPropertyName("id")]
        public string Id { get; set; }
        [JsonPropertyName("name")]
        public string Name { get; set; }
        [JsonPropertyName("picture")]
        public string Picture { get; set; }
        [JsonPropertyName("custom_participant_id")]
        public string CustomParticipantId { get; set; }
        [JsonPropertyName("preset_id")]
        public string PresetId { get; set; }
        [JsonPropertyName("token")]
        public string Token { get; set; }
    }

}
