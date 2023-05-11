using Microsoft.Graph;
using Microsoft.Identity.Client;
using System.Net.Http.Headers;
using System.Text.Json;

namespace WebGraphApplication.Helpers
{
    public class MsalAuthenticationProvider : IAuthenticationProvider
    {
        private IConfidentialClientApplication _clientApplication;
        private string[] _scopes;
        public MsalAuthenticationProvider(IConfidentialClientApplication clientApplication, string[] scopes)
        {
            _clientApplication = clientApplication;
            _scopes = scopes;
        }
        public async Task AuthenticateRequestAsync(HttpRequestMessage request)
        {
            var token = await GenerateTokenFromCredentials(false);
            request.Headers.Authorization = new AuthenticationHeaderValue("bearer", token.access_token);
        }
        public async Task<string> GetTokenAsync()
        {
            AuthenticationResult authResult = null;
            authResult = await _clientApplication.AcquireTokenForClient(_scopes).ExecuteAsync();
            return authResult.AccessToken;
        }
        public static async Task<Office365Token> GenerateTokenFromCredentials(bool graph)
        {
            List<KeyValuePair<string, string>> values = new List<KeyValuePair<string, string>>();
            values.Add(new KeyValuePair<string, string>("client_id", "5ef0bf70-1706-402a-81da-87f2f08d4a15"));
            values.Add(new KeyValuePair<string, string>("resource", graph ? "https://graph.microsoft.com" : "https://graph.microsoft.com"));
            values.Add(new KeyValuePair<string, string>("username", "sinah@2fv4z8.onmicrosoft.com"));
            values.Add(new KeyValuePair<string, string>("password", "Sarasarasarasara11@"));
            values.Add(new KeyValuePair<string, string>("grant_type", "password"));
            values.Add(new KeyValuePair<string, string>("client_secret", "Zps8Q~.0KnhtpIPKJQJJ.fq05C8h1yvB-TEIvcEE"));
            using HttpClient client = new();
            var content = new FormUrlEncodedContent(values);
            var url = $"https://login.microsoftonline.com/4bcc17ec-7c53-4b62-81ef-9537c26b14ac/oauth2/token";
            using var response = await client.PostAsync(url, content);
            var responseString = await response.Content.ReadAsStringAsync();
            Office365Token result = JsonSerializer.Deserialize<Office365Token>(responseString);
            return result;
        }
    }
}
