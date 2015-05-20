using System;
using Newtonsoft.Json;

namespace NestSharp
{
    public class Eta
    {
        [JsonProperty("trip_id")]
        public string TripId { get; set; }

        [JsonProperty("estimated_arrival_window_begin")]
        public DateTime ArrivalWindowBegin { get; set; }

        [JsonProperty("estimated_arrival_window_end")]
        public DateTime ArrivalWindowEnd { get; set; }
    }
}
