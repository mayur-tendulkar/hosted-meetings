using hosted_meetings_backend.Models;
using hosted_meetings_backend.Utils;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Headers;
using System.Net;
using System.Text.Json;
using System.Text;
using Azure.Data.Tables;

namespace hosted_meetings_backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MeetingsController : ControllerBase
    {
        private readonly ILogger<MeetingsController> _logger;
        public MeetingsController(ILogger<MeetingsController> logger)
        {
            _logger = logger;
        }

        [HttpPost(Name = "CreateMeeting")]
        public async Task<IActionResult> Create(Meeting meeting)
        {
            // Check the provided meeting details
            if (string.IsNullOrEmpty(meeting.Title))
                return StatusCode(400);

            var json = JsonSerializer.Serialize(meeting);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var client = new HttpClient();
            var authToken =
                Convert.ToBase64String(Encoding.UTF8.GetBytes($"{Globals.OrganizationId}:{Globals.ApiKey}"));

            client.BaseAddress = new Uri("https://api.cluster.dyte.in/");
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", authToken);

            //Create the meeting using Dyte REST APIs
            var responseMessage = await client.PostAsync("/v2/meetings/", content);
            var responseCode = responseMessage.EnsureSuccessStatusCode();
            if (responseCode.StatusCode != HttpStatusCode.Created)
            {
                return BadRequest(responseCode);
            }

            var responseData = await responseMessage.Content.ReadAsStringAsync();
            var responseObject = JsonSerializer.Deserialize<MeetingResponse>(responseData);


            //Store the meeting using Azure Table Storage
            var tableClient = new TableClient(Globals.AzureStorageUri, Globals.AzureTableForMeetings);
            await tableClient.CreateIfNotExistsAsync();
            var entity = new TableEntity(Globals.OrganizationId, responseObject?.Data?.MeetingId)
            {
                { "Name", meeting?.Title},
                { "CreatedOn", responseObject?.Data?.CreatedOn},
                { "IsRecordOnStart", responseObject?.Data?.IsRecordOnStart},

            };
            
            await tableClient.AddEntityAsync(entity);

            //Return created meeting object
            return Created(string.Empty, responseObject);
        }

    }
}
