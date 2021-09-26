using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Xamarin.Essentials;

namespace GProof.Alerta.Services
{
    public class GeoLocationService : IGeoLocationService
    {
        CancellationTokenSource cts;

        Dictionary<string, double> LatLonDict = new Dictionary<string, double>();
        public async Task<Dictionary<string, double>> GetCurrentLocation()
        {
            try
            {
                
                var request = new GeolocationRequest(GeolocationAccuracy.Best, TimeSpan.FromSeconds(10));
                cts = new CancellationTokenSource();
                var location = await Geolocation.GetLocationAsync(request, cts.Token);

                if (location != null)
                {
                    LatLonDict.Add("Lat", location.Latitude);
                    LatLonDict.Add("Lon", location.Longitude);

                    Console.WriteLine($"Latitude: {location.Latitude}, Longitude: {location.Longitude}, Altitude: {location.Altitude}");
                    
                }
               
            }
            catch (FeatureNotSupportedException fnsEx)
            {
                // Handle not supported on device exception
            }
            catch (FeatureNotEnabledException fneEx)
            {
                // Handle not enabled on device exception
            }
            catch (PermissionException pEx)
            {
                // Handle permission exception
            }
            catch (Exception ex)
            {
                // Unable to get location
            }

            return LatLonDict;
        }
    }
}
