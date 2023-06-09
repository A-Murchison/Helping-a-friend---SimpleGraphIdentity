using ApplicationIdentity;

// Default Scope
string[] scopes = { "https://graph.microsoft.com/.default" };
string tenantId = "common";
string clientId = "3d04380f-2420-4eb9-ba3b-28f07e1ef5f4";

// Create new Token using KPT Identity
Token token = new(tenantId, clientId, scopes);
string? accessToken = token.GetToken().Result;

if (accessToken != null)
{
    Console.WriteLine(accessToken);
}
else
{
    Console.WriteLine("Failed to get access token");
}