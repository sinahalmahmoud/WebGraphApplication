using Newtonsoft.Json;
using System;
using System.Text.Json;

namespace WebGraphApplication
{
    public class Office365Token
    {
        [JsonProperty("token_type")]
        public string token_type { get; set; }

        [JsonProperty("scope")]
        public string scope { get; set; }


        [JsonProperty("expires_in")]
        public string expires_in { get; set; }


        [JsonProperty("ext_expires_in")]
        public string ext_expires_in { get; set; }


        [JsonProperty("expires_on")]
        public string expires_on { get; set; }


        [JsonProperty("not_before")]
        public string not_before { get; set; }

        [JsonProperty("resource")]
        public string resource { get; set; }

        [JsonProperty("access_token")]
        public string access_token { get; set; }

        [JsonProperty("refresh_token")]
        public string refresh_token { get; set; }



        public Office365Token(string token_type, string scope, string expires_in, string ext_expires_in, string expires_on, string not_before,
            string resource, string access_token, string refresh_token)
        {
            this.token_type = token_type;
            this.scope = scope;
            this.expires_in = expires_in;
            this.ext_expires_in = ext_expires_in;
            this.expires_on = expires_on;
            this.not_before = not_before;
            this.resource = resource;
            this.access_token = access_token;
            this.refresh_token = refresh_token;
        }
    }
}
