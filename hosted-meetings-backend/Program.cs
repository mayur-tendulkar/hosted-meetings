
using hosted_meetings_backend.Utils;

namespace hosted_meetings_backend
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            //Fetch and values from appsettings.json
            var apiKey = builder.Configuration.GetSection("DyteCredentials:APIKey").Value;
            if (!string.IsNullOrEmpty(apiKey))
            {
                Globals.ApiKey = apiKey;
            }
            var orgId = builder.Configuration.GetSection("DyteCredentials:OrganizationId").Value;
            if (!string.IsNullOrEmpty(orgId))
            {
                Globals.OrganizationId = orgId;
            }
            var accountName = builder.Configuration.GetSection("AzureCredentials:AccountName").Value;
            if (!string.IsNullOrEmpty(accountName))
            {
                Globals.AzureStorageAccountName = accountName;
            }
            var meetingsTableName = builder.Configuration.GetSection("AzureCredentials:MeetingsTableName").Value;
            if (!string.IsNullOrEmpty(meetingsTableName))
            {
                Globals.AzureTableForMeetings = meetingsTableName;
            }
            var invitesTableName = builder.Configuration.GetSection("AzureCredentials:InvitesTableName").Value;
            if (!string.IsNullOrEmpty(invitesTableName))
            {
                Globals.AzureTableForInvites = invitesTableName;
            }
            var accountKey = builder.Configuration.GetSection("AzureCredentials:StorageAccountKey").Value;
            if (!string.IsNullOrEmpty(accountKey))
            {
                Globals.AzureStorageAccountKey = accountKey;
            }
            var storageUri = builder.Configuration.GetSection("AzureCredentials:StorageUri").Value;
            if (!string.IsNullOrEmpty(storageUri))
            {
                Globals.AzureStorageUri = storageUri;
            }
            


            // Add services to the container.
            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}