using System;
using Newtonsoft.Json;
using System.Collections.Generic;
using Newtonsoft.Json.Converters;
using System.Threading.Tasks;
using System.Net.Http;

namespace NestSharp
{

    public class SmokeCoAlarm
    {
        /// <summary>
        /// Thermostat unique identifier
        /// </summary>
        [JsonProperty ("device_id")]
        public string DeviceId { get;set; }

        /// <summary>
        /// Country and language preference, in IETF Language Tag format
        /// </summary>
        [JsonProperty ("locale")]
        public string Locale { get;set; }

        /// <summary>
        /// Software Version
        /// </summary>
        [JsonProperty ("software_version")]
        public string SoftwareVersion { get;set; }

        /// <summary>
        /// Unique identifier of the structure
        /// </summary>
        [JsonProperty ("structure_id")]
        public string StructureId { get; set; }

        /// <summary>
        /// Display name of the device
        /// </summary>
        [JsonProperty ("name")]
        public string Name { get;set; }

        /// <summary>
        /// Long display name of the device
        /// </summary>
        [JsonProperty ("name_long")]
        public string NameLong { get;set; }

        /// <summary>
        /// Time of the last successful interaction with the Nest service, in ISO 8601 format
        /// </summary>
        [JsonProperty ("last_connection")]
        public DateTime LastConnection { get; set; }

        /// <summary>
        /// Device connection status with the Nest Service
        /// </summary>
        [JsonProperty ("is_online")]
        public bool IsOnline { get;set; }

        /// <summary>
        /// Battery life/health; estimate of time to end of life
        /// </summary>
        [JsonProperty ("battery_health"), JsonConverter (typeof(StringEnumConverter))]
        public BatteryHealth BatteryHealth { get;set; }

        /// <summary>
        /// Smoke alarm status
        /// </summary>
        [JsonProperty ("smoke_alarm_state"), JsonConverter (typeof(StringEnumConverter))]
        public CoAlarmState SmokeAlarmState { get;set; }

        /// <summary>
        /// State of the manual smoke and CO alarm test.
        /// </summary>
        [JsonProperty ("is_manual_test_active")]
        public bool IsManualTestActive { get;set; }

        /// <summary>
        /// Timestamp of the last successful manual test, in ISO 8601 format.
        /// </summary>
        [JsonProperty ("last_manual_test_time")]
        public DateTime LastManualTestTime { get;set; }

        /// <summary>
        /// Indicates device status by color in the Nest app UI; it is an aggregate condition for battery+smoke+co states, and reflects the actual color indicators displayed in the Nest app
        /// </summary>
        [JsonProperty ("ui_color_state"), JsonConverter (typeof(StringEnumConverter))]
        public ColorState UIColorState { get;set; }
    }
    
}
