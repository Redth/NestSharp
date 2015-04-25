using System;
using Newtonsoft.Json;
using System.Collections.Generic;
using Newtonsoft.Json.Converters;
using System.Threading.Tasks;
using System.Net.Http;

namespace NestSharp
{

    public class MetaData
    {
        [JsonProperty ("access_token")]
        public string AccessToken { get;set; }

        [JsonProperty ("client_version")]
        public double ClientVersion { get;set; }
    }
    
}
