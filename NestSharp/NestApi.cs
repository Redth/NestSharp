using System;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Net.Http;
using System.Linq;
using System.Text;
using Newtonsoft.Json.Linq;

namespace NestSharp
{
    public class NestApi
    {
        public NestApi (string clientId, string clientSecret)
        {
            ClientId = clientId;
            ClientSecret = clientSecret;

            http = new HttpClient ();
        }

        const string BASE_URL = "https://developer-api.nest.com/";
        const string AUTHORIZATION_URL = "https://home.nest.com/login/oauth2?client_id={0}&state={1}";
        const string ACCESS_TOKEN_URL  = "https://api.home.nest.com/oauth2/access_token";

        HttpClient http;

        public string ClientId { get; private set; }

        public string ClientSecret { get; private set; }

        public string AccessToken { get; set; }

        public DateTime ExpiresAt { get; set; }


        public string GetAuthorizationUrl ()
        {
            var state = Guid.NewGuid ().ToString ();

            return string.Format (
                AUTHORIZATION_URL,
                ClientId,
                state);            
        }

        public async Task GetAccessToken (string authorizationToken)
        {
            var url = string.Format (ACCESS_TOKEN_URL,
                          ClientId,
                          authorizationToken,
                          ClientSecret);

            var v = new Dictionary<string, string> ();
            v.Add ("client_id", ClientId);
            v.Add ("code", authorizationToken);
            v.Add ("client_secret", ClientSecret);
            v.Add ("grant_type", "authorization_code");

            var r = await http.PostAsync (url, new FormUrlEncodedContent (v));
            var data = await r.Content.ReadAsStringAsync ();

            var json = Newtonsoft.Json.Linq.JObject.Parse (data);

            if (json != null) {

                if (json ["access_token"] != null)
                    AccessToken = json ["access_token"].ToString ();

                if (json ["expires_in"] != null) {
                    var expiresIn = json.Value<long> ("expires_in");

                    ExpiresAt = DateTime.UtcNow.AddSeconds (expiresIn);
                }
            }           
        }

        void CheckAuth ()
        {
            if (string.IsNullOrEmpty (AccessToken)
                || ExpiresAt < DateTime.UtcNow) {
                throw new UnauthorizedAccessException ("Invalid Acess Token");
            }
        }

        public async Task<Devices> GetStructuresAndDevicesAsync ()
        {
            CheckAuth ();

            var url = "https://developer-api.nest.com/?auth={0}";

            var data = await http.GetStringAsync (string.Format (url, AccessToken));

            return JsonConvert.DeserializeObject<Devices> (data);
        }

        public async Task<Devices> GetDevicesAsync ()
        {
            CheckAuth ();

            var url = "https://developer-api.nest.com/devices.json?auth={0}";

            var data = await http.GetStringAsync (string.Format (url, AccessToken));

            return JsonConvert.DeserializeObject<Devices> (data);
        }

        public async Task<Devices> GetStructuresAsync ()
        {
            CheckAuth ();

            var url = "https://developer-api.nest.com/structures.json?auth={0}";

            var data = await http.GetStringAsync (string.Format (url, AccessToken));

            return JsonConvert.DeserializeObject<Devices> (data);
        }
            
        public async Task<Thermostat> GetThermostatAsync (string deviceId)
        {
            CheckAuth ();

            var url = BASE_URL + "devices/thermostats/.json{0}?auth={1}";
                       
            var data = await http.GetStringAsync (string.Format (url, deviceId, AccessToken));

            var thermostats = JsonConvert.DeserializeObject<Dictionary<string, Thermostat>> (data);

            return thermostats.FirstOrDefault ().Value;
        }

        public async Task<JObject> AdjustTemperatureAsync (string deviceId, float degrees, TemperatureScale scale, TemperatureSettingType type = TemperatureSettingType.None)
        {
            var url = BASE_URL + "devices/thermostats/{0}?auth={1}";

            var field = "target_temperature_{0}{1}";

            var tempScale = scale.ToString ().ToLower ();

            var tempType = string.Empty;
            if (type != TemperatureSettingType.None)
                tempType = type.ToString ().ToLower () + "_";
            
            var thermostat = await GetThermostatAsync (deviceId);

            var exMsg = string.Empty;

            if (thermostat.IsUsingEmergencyHeat) {
                exMsg = "Can't adjust target temperature while using emergency heat";
            } else if (thermostat.HvacMode == HvacMode.HeatCool && type == TemperatureSettingType.None) {
                exMsg = "Can't adjust targt temperature while in Heat + Cool Mode, use High/Low TemperatureSettingTypes instead";
            } else if (thermostat.HvacMode != HvacMode.HeatCool && type != TemperatureSettingType.None) {
                exMsg = "Can't adjust target temperature type.ToString () while in Heat or Cool mode.  Use None for TemperatureSettingType instead";
            }
            //TODO: Get the structure instead and check for away
//            else if (1 == 2) { // Check for 'away'
//                exMsg = "Can't adjust target temperature while in Away or Auto-Away mode";
//            }

            if (!string.IsNullOrEmpty (exMsg))
                throw new ArgumentException (exMsg);

            var formattedUrl = string.Format (
                                   url, 
                                   thermostat.DeviceId,
                                   AccessToken);

            var formattedField = string.Format (
                                     field, 
                                     tempType,
                                     tempScale);
                          
            var json = @"{""" + formattedField + @""": " + degrees + "}";


            var r = await http.PutAsync (formattedUrl, new StringContent (
                        json,
                        Encoding.UTF8,
                        "application/json"));

            r.EnsureSuccessStatusCode ();

            var data = await r.Content.ReadAsStringAsync ();

            return JObject.Parse (data);
        }            
    }
}

