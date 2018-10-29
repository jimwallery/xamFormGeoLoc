using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using MvvmHelpers;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace xamFormGeoLoc
{
    public partial class RouteStop : ObservableObject
    {
        string name;
        string location;
        string route;
        double latitude;
        double longitude;

        [JsonProperty("id")]
        public string Name
        {
            get { return name; }
            set
            {
                SetProperty(ref name, value);
            }
        }
        [JsonProperty("gloObjectId")]
        public string Location
        {
            get { return location; }
            set
            {
                SetProperty(ref location, value);
            }
        }

        [JsonProperty("status")]
        public string Route
        {
            get { return route; }
            set
            {
                SetProperty(ref route, value);
            }
        }

        [JsonProperty("gloLatitude")]
        public double Latitude
        {
            get { return latitude; }
            set
            {
                SetProperty(ref latitude, value);
            }
        }

        [JsonProperty("gloLongitude")]
        public double Longitude
        {
            get { return longitude; }
            set
            {
                SetProperty(ref longitude, value);
            }
        }
    }

    public partial class RouteStop
    {
        public static RouteStop[] FromJson(string json) => JsonConvert.DeserializeObject<RouteStop[]>(json, xamFormGeoLoc.Converter.Settings);
    }

    public static class Serialize
    {
        public static string ToJson(this RouteStop[] self) => JsonConvert.SerializeObject(self, xamFormGeoLoc.Converter.Settings);
    }

    internal static class Converter
    {
        public static readonly JsonSerializerSettings Settings = new JsonSerializerSettings
        {
            MetadataPropertyHandling = MetadataPropertyHandling.Ignore,
            DateParseHandling = DateParseHandling.None,
            Converters = {
                new IsoDateTimeConverter { DateTimeStyles = DateTimeStyles.AssumeUniversal }
            },
        };
    }
}
