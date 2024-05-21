using Microsoft.Extensions.Configuration;
using SimpleGraphIdentity.Extensions;
using SimpleGraphIdentity.Models;
using System.Text.Json;

//Build config 
var configuration = new ConfigurationBuilder()
     .AddJsonFile("appsettings.json");
var config = configuration.Build();
var settings = config.GetSection("AppSettings").Get<AppSettings>();


// Default Scope
string[] scopes = { "https://graph.microsoft.com/.default" };
string tenantId = settings.TenantId;
string clientId = settings.ClientId;

// Create new Token
Token token = new(tenantId, clientId, scopes);
string? accessToken = await token.GetToken();

if (accessToken != null)
{
    // Create new GraphClient

    using (var graphClient = new GraphClient(accessToken))
    {
        HttpResponseMessage response = await graphClient.Client.GetAsync("https://graph.microsoft.com/v1.0/me");
        if (response == null || !response.IsSuccessStatusCode)
            throw new Exception("Pass in error message...");

        string json = await response.Content.ReadAsStringAsync();
        GraphUser graphUser = JsonSerializer.Deserialize<GraphUser>(json);

        Console.WriteLine($"Id: {graphUser.Id}");
        Console.WriteLine($"DisplayName: {graphUser.DisplayName}");
        Console.WriteLine($"Mail: {graphUser.Mail}");
    }  
}
else
{
    Console.WriteLine("Failed to get access token");
}