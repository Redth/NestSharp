# NestSharp

This is a feeble attempt at creating a C#/.NET Portable Library to consume data from and interact with NEST devices.

Currently some of the basics work, you can get information about devices, and you can adjust the temperature of a thermostat.  The groundwork is there, and the rest just needs to be added.

## Authorization
NEST uses OAUTH to authorize.  Currently only the PIN (no Callback URI specified in your client) is supported.  When you create a NEST Client in the developer portal, be sure to specify no callback URI.

To authorize using the API, first get the authorization URI:

```csharp
var authUrl = nest.GetAuthorizationUrl ();
```

You'll need to display this to the user in a web browser of some sort, they will accept the permissions they are authorizing, login, and finally be shown a PIN which they'll need to enter back in the application.

When you have the PIN, get an access token:

```csharp
await nest.GetAccessToken (pin);
```
NOTE: Obtaining an access token will return an expiry time for the token, however NEST states that the expiry time is so long-lived that it can be considered indefinite, and as such, there is no refresh-token API to keep the access token current.  You should probably check for HTTP 401 errors on API calls just to be safe.

## NEST APIs

Once you have received your access token, you can start to make requests to the API methods.

```csharp
// Fetch devices 
var devices = await nest.GetDevicesAsync ();

// Loop through the devices
foreach (var t in devices.Thermostats) {

	// Set the temperature on our thermostats
	await nest.AdjustTemperatureAsync (
			t.DeviceId,
			21.5f,
			TemperatureScale.C);                
}
```

More methods and the rest of the data object models are coming soon!