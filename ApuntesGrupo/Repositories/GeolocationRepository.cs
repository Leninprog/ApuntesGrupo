using System;
using System.Threading;
using System.Threading.Tasks;

namespace ApuntesGrupo.Repositories
{
    class GeolocationRepository
    {
        private CancellationTokenSource _cancelTokenSource;
        private bool _isCheckingLocation;

        public async Task<Location> GetCurrentLocationAsync()
        {
            try
            {
                _isCheckingLocation = true;

                GeolocationRequest request = new GeolocationRequest(GeolocationAccuracy.Medium, TimeSpan.FromSeconds(10));
                _cancelTokenSource = new CancellationTokenSource();

                Location location = await Geolocation.Default.GetLocationAsync(request, _cancelTokenSource.Token);

                if (location != null)
                    return location;
            }
            catch (FeatureNotSupportedException fnsEx)
            {
                // La geolocalización no está soportada en el dispositivo
                Console.WriteLine($"[Geolocation] No soportado: {fnsEx.Message}");
            }
            catch (PermissionException pEx)
            {
                Console.WriteLine($"[Geolocation] Sin permisos: {pEx.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[Geolocation] Error: {ex.Message}");
            }
            finally
            {
                _isCheckingLocation = false;
            }
            return new Location
            {
                Latitude = 46.48,
                Longitude = 7.44
            };
        }

        public void CancelRequest()
        {
            if (_isCheckingLocation && _cancelTokenSource?.IsCancellationRequested == false)
            {
                _cancelTokenSource.Cancel();
            }
        }
    }
}
