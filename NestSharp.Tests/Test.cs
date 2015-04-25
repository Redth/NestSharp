using NUnit.Framework;
using System;
using System.Threading.Tasks;
using System.IO;

namespace NestSharp.Tests
{
    // This doesn't actually run as a test, just some code to show how to use the API for now
    [TestFixture ()]
    public class Tests
    {
        const string CLIENT_ID = "";
        const string CLIENT_SECRET = "";

        [Test ()]
        public async Task Test ()
        {
            var accessToken = string.Empty;

            var file = Path.Combine (AppDomain.CurrentDomain.BaseDirectory, "accesstoken.txt");

            if (File.Exists (file))
                accessToken = File.ReadAllText (file);

            var nest = new NestApi (CLIENT_ID, CLIENT_SECRET) {
                AccessToken = accessToken,
                ExpiresAt = DateTime.UtcNow.AddYears (1)
            };

            if (string.IsNullOrEmpty (nest.AccessToken)) {

                Console.WriteLine ("Login to the Web Browser and authorize...");

                // Get the URL to load in a browser for the PIN
                var authUrl = nest.GetAuthorizationUrl ();

                System.Diagnostics.Process.Start (authUrl);

                Console.WriteLine ("Enter your PIN:");

                // Read back in the PIN
                var authToken = Console.ReadLine ();

                // Get an access token to use with the API
                await nest.GetAccessToken (authToken);

                File.WriteAllText (file, nest.AccessToken);
            }

            Console.WriteLine ("Fetching Devices...");

            // Fetch devices 
            var devices = await nest.GetDevicesAsync ();

            // Loop through the devices
            foreach (var t in devices.Thermostats) {

                var thermostatId = t.Value.DeviceId;

                await nest.AdjustTemperatureAsync (
                    thermostatId,
                    21.5f,
                    TemperatureScale.C);                
            }
        }
    }
}

