namespace hosted_meetings_backend.Utils
{
    /// <summary>
    /// This class is being used for holding global values from appsettings.json
    /// </summary>
    public static class Globals
    {
        //Dyte Specific Variables
        public static string? OrganizationId { get; set; }
        public static string? ApiKey { get; set; }

        //Azure Specific Variables
        public static string? AzureStorageAccountName { get; set; }
        public static string? AzureStorageAccountKey { get; set; }
        public static string? AzureStorageUri { get; set; }
        public static string? AzureTableForMeetings { get; set; }
        public static string? AzureTableForInvites { get; set; }

        
    }
}
