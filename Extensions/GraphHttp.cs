namespace SimpleGraphIdentity.Extensions;

//We need to implement IDisposable as we have a disposable property 'HttpClient' 
internal class GraphClient : IDisposable
{
    private string Token { get; set; }
    public HttpClient Client { get; }

    public GraphClient(string token)
    {
        //TODO: add in some cool logic to help with the HTTP client integrate with Graph :D
        Token = token;
        Client = new HttpClient();
        Client.DefaultRequestHeaders.Authorization = new("Bearer", Token);
    }

    public void Dispose()
    {
        Token = "";
        Client.Dispose();
    }
}
