using Azure.Data.Tables;
using hosted_meetings_backend.Utils;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Headers;
using System.Net;
using System.Text.Json;
using System.Text;
using hosted_meetings_backend.Models;

namespace hosted_meetings_backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ParticipantsController : ControllerBase
    {
        private readonly ILogger<ParticipantsController> _logger;
        public ParticipantsController(ILogger<ParticipantsController> logger)
        {
            _logger = logger;
        }
        

        [HttpPost(Name = "JoinMeeting")]
        public async Task<IActionResult> Join(string meetingId, string participantEmail, string inviteCode)
        {
            var tableClient = new TableClient(Globals.AzureStorageUri, Globals.AzureTableForInvites);
            await tableClient.CreateIfNotExistsAsync();
            var queryResultsFilter = tableClient.Query<TableEntity>(e => e.PartitionKey == meetingId && e.RowKey == participantEmail);

            foreach (var entity in queryResultsFilter)
            {
                if (entity["Code"].ToString() == inviteCode)
                {
                    var client = new HttpClient();
                    var authToken =
                        Convert.ToBase64String(Encoding.UTF8.GetBytes($"{Globals.OrganizationId}:{Globals.ApiKey}"));

                    client.BaseAddress = new Uri("https://api.cluster.dyte.in/");
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", authToken);

                    var attendee = new
                    {
                        name = entity["Name"].ToString(),
                        client_specific_id = entity["ClientSpecificId"].ToString(),
                        preset_name = entity["Preset"].ToString(),
                        picture = entity["PictureUrl"].ToString()
                    };

                    var json = JsonSerializer.Serialize(attendee);
                    var content = new StringContent(json, Encoding.UTF8, "application/json");

                    //Create the meeting using Dyte REST APIs
                    var responseMessage = await client.PostAsync($"/v2/meetings/{meetingId}/participants", content);
                    var responseCode = responseMessage.EnsureSuccessStatusCode();
                    if (responseCode.StatusCode != HttpStatusCode.Created)
                    {
                        return BadRequest(responseCode);
                    }

                    var responseData = await responseMessage.Content.ReadAsStringAsync();
                    var responseObject = JsonSerializer.Deserialize<ParticipantResponse>(responseData);
                    return Created(string.Empty, responseObject);
                }
                else
                {
                    return NotFound();
                }
            }
            return NotFound();
        }
    }
}
