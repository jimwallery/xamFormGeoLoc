using System.Collections.Generic;
using xamFormGeoLoc.ViewModels;
using Xamarin.Forms.Maps;
using Xamarin.Forms;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using MvvmHelpers;

namespace xamFormGeoLoc
{
    public class CustomMap : Map
    {
        //public ObservableRangeCollection<CustomPin> CustomPins { get; set; }
        public ObservableRangeCollection<CustomPin> PinCollection { get; set; }

        public static readonly BindableProperty MapPinsProperty = BindableProperty.Create(
             nameof(Pins),
             typeof(ObservableRangeCollection<CustomPin>),
             typeof(CustomMap),
             new ObservableRangeCollection<CustomPin>(),
             propertyChanged: (b, o, n) =>
             {
                 var bindable = (CustomMap)b;
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
                                         bindable.Pins.Remove((CustomPin)item);
                                 if (e.NewItems != null)
                                     foreach (var item in e.NewItems)
                                         bindable.Pins.Add((CustomPin)item);
                                         //bindable.PinCollection.Add((CustomPin)item);
                                 break;
                             case NotifyCollectionChangedAction.Reset:
                                 bindable.Pins.Clear();
                                 break;
                         }
                     });
                 };
             });

        public List<CustomPin> MapPins { get; set; }
        //public List<CustomPin> CustomPins { get; set; }


        public static readonly BindableProperty MapPositionProperty = BindableProperty.Create(
             nameof(MapPosition),
             typeof(Position),
             typeof(CustomMap),
             new Position(0, 0),
             propertyChanged: (b, o, n) =>
             {
                 ((CustomMap)b).MoveToRegion(MapSpan.FromCenterAndRadius(
                      (Position)n,
                      Distance.FromMiles(1)));
             });

        public Position MapPosition { get; set; }
    }
}
