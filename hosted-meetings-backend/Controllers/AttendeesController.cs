using Azure.Data.Tables;
using hosted_meetings_backend.Models;
using hosted_meetings_backend.Utils;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace hosted_meetings_backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AttendeesController : ControllerBase
    {
        private readonly ILogger<AttendeesController> _logger;
        public AttendeesController(ILogger<AttendeesController> logger)
        {
            _logger = logger;
        }

        [HttpPost]
        public async Task<IActionResult> Invite(String meetingId, List<Attendee> attendees)
        {
            if (string.IsNullOrEmpty(meetingId) || attendees.Count == 0)
                return StatusCode(400);

            //Store the meeting invite using Azure Table Storage
            var tableClient = new TableClient(Globals.AzureStorageUri, Globals.AzureTableForInvites);
            await tableClient.CreateIfNotExistsAsync();


            foreach (var attendee in attendees)
            {
                var random = new Random();
                var entity = new TableEntity(meetingId, attendee.Email)
                {
                    { "Name", attendee.Name },
                    { "Code", random.Next(0000, 9999) },
                    { "Preset", attendee.PresetName },
                    { "PictureUrl", attendee.PictureUrl},
                    { "ClientSpecificId", attendee.ClientSpecificId}

                };
                await tableClient.AddEntityAsync(entity);
            }

            return Created(string.Empty, attendees);

        }
    }
}
