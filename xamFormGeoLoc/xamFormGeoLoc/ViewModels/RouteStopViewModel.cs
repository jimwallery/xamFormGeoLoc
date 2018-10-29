using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;
using xamFormGeoLoc;
using xamFormGeoLoc.Views;
using System.Windows.Input;
using MvvmHelpers;
using Xamarin.Forms;
using Xamarin.Essentials;
using System.Net.Http;
using Newtonsoft.Json;
using System.Linq;
using Xamarin.Forms.Maps;
using System.Collections.ObjectModel;

namespace xamFormGeoLoc.ViewModels
{
    public class RouteStopViewModel : BaseViewModel
    {
        public RouteStopViewModel()
        {
            Title = "Route Stops";
            RouteStops = new ObservableRangeCollection<RouteStop>();

            RouteStops.CollectionChanged += (sender, args) =>
            {
                Debug.WriteLine("Item {args.Action}");
            };

            GetRouteStopsCommand = new Command(async () => await GetRouteStopsAsync());
            GetClosestCommand = new Command(async () => await GetClosestAsync());

            var pin = new CustomPin
            {
                Position = new Position(41.411835, -75.665245),
                Type = PinType.Place,
                Label = "The Office",
                Address = "Scranton PA",
                Id = "Xamarin2",
                Url = "http://xamarin.com/about/"
            };



            PinCollection.Add(new CustomPin() { Position = MyPosition, Type = PinType.Place, Label = "Xamarin San Francisco Office", Address = "394 Pacific Ave, San Francisco CA", Id = "Xamarin", Url = "http://xamarin.com/about/" });
            //PinCollection.Add(pin);
            

            //customPins.Add(new CustomPin() { Position = MyPosition, Type = PinType.Place, Label = "Xamarin San Francisco Office", Address = "394 Pacific Ave, San Francisco CA", Id = "Xamarin", Url = "http://xamarin.com/about/" });

            var CustomMap = new CustomMap();
            CustomMap.MoveToRegion(MapSpan.FromCenterAndRadius(new Position(MyPosition.Latitude, MyPosition.Longitude), Distance.FromMiles(1)));

        }


        public ObservableRangeCollection<RouteStop> RouteStops { get; set; }
        public Command GetRouteStopsCommand { get; }
        public Command GetClosestCommand { get; }

        private Position _myPosition = new Position(41.411835, -75.665245);
        public Position MyPosition { get { return _myPosition; } set { _myPosition = value; OnPropertyChanged(); } }

        
        
        


        private ObservableRangeCollection<CustomPin> _pinCollection = new ObservableRangeCollection<CustomPin>();
        public ObservableRangeCollection<CustomPin> PinCollection { get { return _pinCollection; } set { _pinCollection = value; OnPropertyChanged(); } }

        //private ObservableCollection<CustomPin> customPins = new ObservableCollection<CustomPin> ();
        //public ObservableCollection<CustomPin> CustomPins { get { return customPins; } set { customPins = value; OnPropertyChanged(); } }

        async Task GetClosestAsync()
        {
            if (RouteStops.Count == 0)
                return;

            if (IsBusy)
                return;
            try
            {
                var location = await Geolocation.GetLastKnownLocationAsync();
                if (location == null)
                {
                    location = await Geolocation.GetLocationAsync(new GeolocationRequest
                    {
                        DesiredAccuracy = GeolocationAccuracy.Medium,
                        Timeout = TimeSpan.FromSeconds(30)
                    });
                }

                var first = RouteStops.OrderBy(m => location.CalculateDistance(
                    new Location(m.Latitude, m.Longitude), DistanceUnits.Miles))
                    .FirstOrDefault();

                await Application.Current.MainPage.DisplayAlert("", first.Name + " " +
                    first.Location  + " " + first.Latitude + " " + first.Longitude, "OK");

                var _myPosition = new Position(first.Latitude, first.Longitude);
                





}
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Something is wrong",
                       "Unable to get location! :(", "OK");
            }
        }

        public async Task GetRouteStopsAsync()
        {
            if (IsBusy)
                return;
            try
            {

                var client = new HttpClient();
                var json = await client.GetStringAsync("https://catchall02.azurewebsites.net/tables/geolocationobject?ZUMO-API-VERSION=2.0.0");
                var all = RouteStop.FromJson(json);
                RouteStops.ReplaceRange(all);
                Title = $"Route Stops ({RouteStops.Count})";
                
            }
            catch (System.Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Error","Something went wrong", "Cancel");
            }
            finally
            {
                IsBusy = false;
            }
        }
    }
}
