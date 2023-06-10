using Newtonsoft.Json.Linq;

namespace GraphHttp;

class GraphClient {
    // Properties
    public string Token { get; set; }
    public HttpClient Client = new();

    // Constructor
    public GraphClient(string Token) {
        this.Token = Token;
        this.Client.DefaultRequestHeaders.Authorization = new("Bearer", this.Token);
    }
}

class GraphUser {
    // Properties
    public string Id { get; set; }
    public string DisplayName { get; set; }
    public string Mail { get; set; }

    // Constructor
    public GraphUser(GraphClient GraphClient){
        HttpResponseMessage response = GraphClient.Client.GetAsync("https://graph.microsoft.com/v1.0/me").Result;
        string json = response.Content.ReadAsStringAsync().Result;

        JObject jObject = JObject.Parse(json);

        if (jObject["id"] != null) {
            this.Id = jObject["id"]!.ToString();
        }
        else {
            this.Id = string.Empty;
        }

        if (jObject["displayName"] != null) {
            this.DisplayName = jObject["displayName"]!.ToString();
        }
        else {
            this.DisplayName = string.Empty;
        }

        if (jObject["mail"] != null) {
            this.Mail = jObject["mail"]!.ToString();
        }
        else {
            this.Mail = string.Empty;
        }
    }
}