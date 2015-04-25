using System;
using Newtonsoft.Json;
using System.Collections.Generic;
using Newtonsoft.Json.Converters;
using System.Threading.Tasks;
using System.Net.Http;

namespace NestSharp
{
    public class Devices
    {
        [JsonProperty ("thermostats")]
        public Dictionary<string, Thermostat> Thermostats { get;set; }

        [JsonProperty ("smoke_co_alarms")]
        public Dictionary<string, SmokeCoAlarm> SmokeCoAlarms { get;set; }
    }
}
