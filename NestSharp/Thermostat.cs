using System;
using Newtonsoft.Json;
using System.Collections.Generic;
using Newtonsoft.Json.Converters;
using System.Threading.Tasks;
using System.Net.Http;

namespace NestSharp
{

    public class Thermostat
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
        /// System ability to cool (AC)
        /// </summary>
        [JsonProperty ("can_cool")]
        public bool CanCool { get; set; }

        /// <summary>
        /// System ability to heat
        /// </summary>
        [JsonProperty ("can_heat")]
        public bool CanHeat { get; set; }

        /// <summary>
        /// Emergency Heat status in systems with heat pumps
        /// </summary>
        [JsonProperty ("is_using_emergency_heat")]
        public bool IsUsingEmergencyHeat { get; set; }

        /// <summary>
        /// System ability to control the fan separately from heating or cooling
        /// </summary>
        [JsonProperty ("has_fan")]
        public bool HasFan { get;set; }

        /// <summary>
        /// Indicates if the fan timer is engaged; used with 'fan_timer_timeout' to turn on the fan for a (user-specified) preset duration
        /// </summary>
        [JsonProperty ("fan_timer_active")]
        public bool FanTimerActive { get;set; }

        /// <summary>
        /// Timestamp, showing when the fan timer reaches 0 (end of timer duration), in ISO 8601 format
        /// </summary>
        [JsonProperty ("fan_timer_timeout")]
        public DateTime FanTimerTimeout { get;set; }

        /// <summary>
        /// Displayed when users choose an energy-saving temperature
        /// </summary>
        [JsonProperty ("has_leaf")]
        public bool HasLeaf { get;set; }

        /// <summary>
        /// Celsius or Fahrenheit; used with temperature display
        /// </summary>
        [JsonProperty ("temperature_scale"), JsonConverter (typeof(StringEnumConverter))]
        public TemperatureScale TemperatureScale { get;set; }

        /// <summary>
        /// Desired temperature, displayed in whole degrees Fahrenheit (1°F)
        /// </summary>
        [JsonProperty ("target_temperature_f")]
        public float TargetTemperatureFarenheit { get;set; }

        /// <summary>
        /// Desired temperature, displayed in half degrees Celsius (0.5°C)
        /// </summary>
        [JsonProperty ("target_temperature_c")]
        public float TargetTemperatureCelsius { get;set; }

        /// <summary>
        /// Maximum target temperature, displayed in whole degrees Fahrenheit (1°F); used with Heat • Cool mode
        /// </summary>
        [JsonProperty ("target_temperature_high_f")]
        public float TargetTemperatureHighFarenheit { get;set; }

        /// <summary>
        /// Maximum target temperature, displayed in half degrees Celsius (0.5°C); used with Heat • Cool mode
        /// </summary>
        [JsonProperty ("target_temperature_high_c")]
        public float TargetTemperatureHighCelsius { get;set; }

        /// <summary>
        /// Minimum target temperature, displayed in whole degrees Fahrenheit (1°F); used with Heat • Cool mode
        /// </summary>
        [JsonProperty ("target_temperature_low_f")]
        public float TargetTemperatureLowFarenheit { get;set; }

        /// <summary>
        /// Minimum target temperature, displayed in half degrees Celsius (0.5°C); used with Heat • Cool mode
        /// </summary>
        [JsonProperty ("target_temperature_low_c")]
        public float TargetTemperatureLowCelsius { get;set; }

        /// <summary>
        /// Maximum 'away' temperature, displayed in whole degrees Fahrenheit (1°F)
        /// </summary>
        [JsonProperty ("away_temperature_high_f")]
        public float AwayTemperatureHighFarenheit { get;set; }

        /// <summary>
        /// Maximum 'away' temperature, displayed in half degrees Celsius (0.5°C)
        /// </summary>
        [JsonProperty ("away_temperature_high_c")]
        public float AwayTemperatureHighCelsius { get;set; }

        /// <summary>
        /// Minimum 'away' temperature, displayed in whole degrees Fahrenheit (1°F)
        /// </summary>
        [JsonProperty ("away_temperature_low_f")]
        public float AwayTemperatureLowFarenheit { get;set; }

        /// <summary>
        /// Minimum 'away' temperature, displayed in half degrees Celsius (0.5°C)
        /// </summary>
        [JsonProperty ("away_temperature_low_c")]
        public float AwayTemperatureLowCelsius { get;set; }

        /// <summary>
        /// Indicates HVAC system heating/cooling modes; for systems with both heating and cooling capability, use 'heat-cool': (Heat • Cool mode)
        /// </summary>
        [JsonProperty ("hvac_mode"), JsonConverter (typeof(StringEnumConverter))]
        public HvacMode HvacMode { get;set; }

        /// <summary>
        /// Temperature, measured at the device, in whole degrees Fahrenheit (1°f)
        /// </summary>
        [JsonProperty ("ambient_temperature_f")]
        public float AmbientTemperatureFarenheight { get;set; }

        /// <summary>
        /// Temperature, measured at the device, in half degrees Celsius (0.5°C)
        /// </summary>
        [JsonProperty ("ambient_temperature_c")]
        public float AmbientTemperatureCelsius { get;set; }

        /// <summary>
        /// Humidity, in percent (%) format, measured at the device.
        /// </summary>
        [JsonProperty ("humidity")]
        public float Humidity { get;set; }

        /// <summary>
        /// Device locked status with the Nest Service
        /// </summary>
        [JsonProperty("is_locked")]
        public bool IsLocked { get; set; }

        /// <summary>
        /// Temperature, minimum at the device, in whole degrees Fahrenheit (1°f)
        /// </summary>
        [JsonProperty("locked_temp_min_f")]
        public float LockedTempMinimumFarenheit { get; set; }

        /// <summary>
        /// Temperature, maximum at the device, in whole degrees Fahrenheit (1°f)
        /// </summary>
        [JsonProperty("locked_temp_max_f")]
        public float LockedTempMaximumFarenheit { get; set; }

        /// <summary>
        /// Display name of the device
        /// </summary>
        [JsonProperty("where_name")]
        public string WhereName { get; set; }

        /// <summary>
        /// Display label of the device
        /// </summary>
        [JsonProperty("label")]
        public string Label { get; set; }
    }
    
}
