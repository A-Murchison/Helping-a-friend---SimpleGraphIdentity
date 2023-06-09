using Microsoft.Identity.Client;
using Microsoft.Identity.Client.Extensions.Msal;

namespace ApplicationIdentity;

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
        this.TenantId = tenantId;
        this.ClientId = clientId;
        this.Scopes = scopes;
        this.CacheName = "userCache";
        this.CachePath = ".\\cache";
        this.Application = PublicClientApplicationBuilder
            .Create(this.ClientId)
            .WithAuthority(AzureCloudInstance.AzurePublic, this.TenantId)
            .WithRedirectUri("http://localhost")
            .Build();

        // Create the Cache directory, file and helper
        StorageCreationProperties storageProperties = new StorageCreationPropertiesBuilder(this.CacheName, this.CachePath).Build();

        MsalCacheHelper cacheHelper = MsalCacheHelper.CreateAsync(storageProperties).Result;
        cacheHelper.RegisterCache(this.Application.UserTokenCache);
    }

    public async Task<string?> GetToken()
    {
        var accounts = await this.Application.GetAccountsAsync();

        AuthenticationResult? result = null;
        try
        {
            result = await this.Application.AcquireTokenSilent(this.Scopes, accounts.FirstOrDefault()).ExecuteAsync();
        }
        catch (MsalUiRequiredException ex)
        {
            Console.WriteLine($"MsalUiRequiredException: {ex.Message}");

            try
            {
                result = await this.Application.AcquireTokenInteractive(this.Scopes).ExecuteAsync();
            }
            catch (MsalException msalex)
            {
                Console.WriteLine($"Error Acquiring Token:{System.Environment.NewLine}{msalex}");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error Acquiring Token Silently:{System.Environment.NewLine}{ex}");
            return null;
        }

        if (result != null)
        {
            return result.AccessToken;
        }
        else return null;
    }
}