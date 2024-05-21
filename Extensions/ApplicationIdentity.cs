using Microsoft.Identity.Client;
using Microsoft.Identity.Client.Extensions.Msal;

namespace SimpleGraphIdentity.Extensions;

class Token
{
    public string TenantId { get; set; }
    public string ClientId { get; set; }
    public string[] Scopes { get; set; }
    public string CacheName { get; set; }
    public string CachePath { get; set; }
    public IPublicClientApplication Application { get; set; }

    public Token(string tenantId, string clientId, string[] scopes)
    {
        TenantId = tenantId;
        ClientId = clientId;
        Scopes = scopes;
        CacheName = "userCache";
        CachePath = ".\\cache";
        Application = PublicClientApplicationBuilder
            .Create(ClientId)
            .WithAuthority(AzureCloudInstance.AzurePublic, TenantId)
            .WithRedirectUri("http://localhost")
            .Build();

        // Create the Cache directory, file and helper
        StorageCreationProperties storageProperties = new StorageCreationPropertiesBuilder(CacheName, CachePath).Build();

        MsalCacheHelper cacheHelper = MsalCacheHelper.CreateAsync(storageProperties).Result;
        cacheHelper.RegisterCache(Application.UserTokenCache);
    }

    public async Task<string?> GetToken()
    {
        //TODO: Check if the token is in the cache first :)

        var accounts = await Application.GetAccountsAsync();

        AuthenticationResult? result = null;
        try
        {
            result = await Application.AcquireTokenSilent(Scopes, accounts.FirstOrDefault()).ExecuteAsync();
        }
        catch (MsalUiRequiredException ex)
        {
            Console.WriteLine($"MsalUiRequiredException: {ex.Message}");

            try
            {
                result = await Application.AcquireTokenInteractive(Scopes).ExecuteAsync();
            }
            catch (MsalException msalex)
            {
                Console.WriteLine($"Error Acquiring Token:{Environment.NewLine}{msalex}");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error Acquiring Token Silently:{Environment.NewLine}{ex}");
            return null;
        }

        if (result != null)
        {
            return result.AccessToken;
        }
        else return null;
    }
}