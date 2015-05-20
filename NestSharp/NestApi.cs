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

        public async Task<string> GetAccessToken (string authorizationToken)
        {
            var v = new Dictionary<string, string> ();
            v.Add ("client_id", ClientId);
            v.Add ("code", authorizationToken);
            v.Add ("client_secret", ClientSecret);
            v.Add ("grant_type", "authorization_code");

            var r = await http.PostAsync(ACCESS_TOKEN_URL, new FormUrlEncodedContent(v));
            var data = await r.Content.ReadAsStringAsync ();

            var json = JObject.Parse (data);

            if (json != null) {

                if (json ["access_token"] != null)
                    AccessToken = json ["access_token"].ToString ();

                if (json ["expires_in"] != null) {
                    var expiresIn = json.Value<long> ("expires_in");

                    ExpiresAt = DateTime.UtcNow.AddSeconds (expiresIn);
                }
            }   

            return AccessToken;
        }

        void CheckAuth ()
        {
            if (string.IsNullOrEmpty (AccessToken)
                || ExpiresAt < DateTime.UtcNow) {
                throw new UnauthorizedAccessException ("Invalid Acess Token");
            }
        }

        //TODO: Get the right data enclosure for this call
//        public async Task<Devices> GetStructuresAndDevicesAsync ()
//        {
//            CheckAuth ();
//
//            var url = "https://developer-api.nest.com/?auth={0}";
//
//            var data = await http.GetStringAsync (string.Format (url, AccessToken));
//
//            return JsonConvert.DeserializeObject<Devices> (data);
//        }

        public async Task<Devices> GetDevicesAsync ()
        {
            CheckAuth ();

            var url = "https://developer-api.nest.com/devices.json?auth={0}";

            var data = await http.GetStringAsync (string.Format (url, AccessToken));

            return JsonConvert.DeserializeObject<Devices> (data);
        }

        public async Task<Dictionary<string, Structure>> GetStructuresAsync ()
        {
            CheckAuth ();

            var url = "https://developer-api.nest.com/structures.json?auth={0}";

            var data = await http.GetStringAsync (string.Format (url, AccessToken));

            return JsonConvert.DeserializeObject<Dictionary<string, Structure>> (data);
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

        /// <summary>
        /// Sets the ETA for the structure with the given structureId.
        /// </summary>
        /// <param name="structureId">The structureId to set the ETA for.</param>
        /// <param name="tripId">A unique ID for the trip. If you want to update an ETA the same tripId must be used.</param>
        /// <param name="arrivalWindowBegin"></param>
        /// <param name="arrivalWindowEnd"></param>
        /// <returns>True on success.</returns>
        /// <exception cref=""></exception>
        public async Task<bool> SetETA(string structureId, string tripId, DateTime arrivalWindowBegin, DateTime arrivalWindowEnd) {
            CheckAuth();
            
            const string url = BASE_URL + "structures/{0}/eta.json?auth={1}";
            var formattedUrl = string.Format(url, structureId, AccessToken);

            var structure = (await GetStructuresAsync()).FirstOrDefault(x => x.Key == structureId).Value;

            if (structure == null) {
                throw new ArgumentException("Unknown Structure ID", structureId);
            }
            if (structure.Away == Away.Home) {
                throw new InvalidOperationException("Can't set ETA when structure is  in Home mode");
            }

            var json = JsonConvert.SerializeObject(new Eta {TripId = tripId, ArrivalWindowBegin = arrivalWindowBegin.ToUniversalTime(), ArrivalWindowEnd = arrivalWindowEnd.ToUniversalTime()});

            var r = await http.PutAsync(formattedUrl, new StringContent(json, Encoding.UTF8, "application/json"));
            r.EnsureSuccessStatusCode();

            return true;
        }
    }
}

