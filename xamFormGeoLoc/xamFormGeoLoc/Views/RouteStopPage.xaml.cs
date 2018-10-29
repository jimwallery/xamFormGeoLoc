using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using xamFormGeoLoc.ViewModels;
using xamFormGeoLoc;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Xamarin.Forms.Maps;
using MvvmHelpers;

namespace xamFormGeoLoc.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class RouteStopPage : ContentPage
	{
        RouteStopViewModel ViewModel => vm ?? (vm = BindingContext as RouteStopViewModel);
        RouteStopViewModel vm;

        public RouteStopPage ()
		{
            BindingContext = new RouteStopViewModel();
            InitializeComponent ();



            var pin = new CustomPin
            {
                Type = PinType.Place,
                Position = new Position(41.411835, -75.665245),
                Label = "The Office",
                Address = "Scranton PA",
                Id = "Xamarin",
                Url = "http://xamarin.com/about/"
            };

           // customMap.CustomPins = new List<CustomPin> { pin };
            customMap.PinCollection = new ObservableRangeCollection<CustomPin> { pin };

            customMap.Pins.Add(pin);
            //customMap.MoveToRegion(MapSpan.FromCenterAndRadius(new Position(37.79752, -122.40183), Distance.FromMiles(1.0)));
        }
	}
}