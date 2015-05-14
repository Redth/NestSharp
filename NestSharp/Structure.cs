using System;
using Newtonsoft.Json;
using System.Collections.Generic;
using Newtonsoft.Json.Converters;
using System.Threading.Tasks;
using System.Net.Http;

namespace NestSharp
{
    public class Structure
    {
        [JsonProperty ("structure_id")]
        public string StructureId { get;set; }

        [JsonProperty ("thermostats")]
        public List<string> Thermostats { get;set; }

        [JsonProperty ("smoke_co_alarms")]
        public List<string> SmokeCoAlarms { get;set; }

        [JsonProperty ("away"), JsonConverter (typeof(StringEnumConverter))]
        public Away Away { get; set; }

        [JsonProperty ("name")]
        public string Name { get;set; }

        [JsonProperty ("country_code")]
        public string CountryCode { get;set; }

        [JsonProperty ("postal_code")]
        public string PostalCode { get;set; }

        [JsonProperty ("peak_period_start_time")]
        public DateTime PeakPeriodStartTime { get; set; }

        [JsonProperty ("peak_period_end_time")]
        public DateTime PeakPeriodEndTime { get; set; }

        [JsonProperty ("time_zone")]
        public string Timezone { get;set; }

        [JsonProperty("eta")]
        public Eta Eta { get; set; }

        //TODO: Implement devices and eta fields
    }
}
