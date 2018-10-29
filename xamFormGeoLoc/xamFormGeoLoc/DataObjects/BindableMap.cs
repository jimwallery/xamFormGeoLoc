using MvvmHelpers;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Text;
using xamFormGeoLoc.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Maps;

namespace xamFormGeoLoc
{
    public class BindableMap : Map
    {
        
        public static readonly BindableProperty MapPinsProperty = BindableProperty.Create(
                  nameof(Pins),
                  typeof(ObservableRangeCollection<CustomPin>),
                  typeof(BindableMap),
                  new ObservableRangeCollection<CustomPin>(),
                  propertyChanged: (b, o, n) =>
                  {
                      var bindable = (BindableMap)b;
                      bindable.Pins.Clear();

                      var collection = (ObservableRangeCollection<CustomPin>)n;
                      foreach (var item in collection)
                          bindable.Pins.Add(item);
                      collection.CollectionChanged += (sender, e) =>
                      {
                          Device.BeginInvokeOnMainThread(() =>
                          {
                              switch (e.Action)
                              {
                                  case NotifyCollectionChangedAction.Add:
                                  case NotifyCollectionChangedAction.Replace:
                                  case NotifyCollectionChangedAction.Remove:
                                      if (e.OldItems != null)
                                          foreach (var item in e.OldItems)
                                              bindable.Pins.Remove((Pin)item);
                                      if (e.NewItems != null)
                                          foreach (var item in e.NewItems)
                                              bindable.Pins.Add((Pin)item);
                                      break;
                                  case NotifyCollectionChangedAction.Reset:
                                      bindable.Pins.Clear();
                                      break;
                              }
                          });
                      };
                  });
        public IList<CustomPin> MapPins { get; set; }

        public static readonly BindableProperty MapPositionProperty = BindableProperty.Create(
                 nameof(MapPosition),
                 typeof(Position),
                 typeof(BindableMap),
                 new Position(0, 0),
                 propertyChanged: (b, o, n) =>
                 {
                     ((BindableMap)b).MoveToRegion(MapSpan.FromCenterAndRadius(
                          (Position)n,
                          Distance.FromMiles(1)));
                 });

        public Position MapPosition { get; set; }

        public List<Position> RouteCoordinates { get; set; }
        public List<CustomPin> CustomPins { get; set; }

        public BindableMap()
        {
            RouteCoordinates = new List<Position>();
            CustomPins = new List<CustomPin>();
            //var bindableMap = new BindableMap
            //{
            //    MapType = MapType.Street,
            //    WidthRequest = App.ScreenWidth,
            //    HeightRequest = App.ScreenHeight
            //};

            //var pin = new CustomPin
            //{
            //    Type = PinType.Place,
            //    Position = new Position(37.79752, -122.40183),
            //    Label = "Xamarin San Francisco Office",
            //    Address = "394 Pacific Ave, San Francisco CA",
            //    Id = "Xamarin",
            //    Url = "http://xamarin.com/about/"
            //};

            //bindableMap.CustomPins = new List<CustomPin> { pin };
            //bindableMap.Pins.Add(pin);
            //bindableMap.MoveToRegion(MapSpan.FromCenterAndRadius(new Position(37.79752, -122.40183), Distance.FromMiles(1.0)));
        }


    }
}