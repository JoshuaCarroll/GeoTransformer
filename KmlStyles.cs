using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SharpKml.Base;
using SharpKml.Dom;

namespace GeoTransformer
{
    internal static class KmlStyles
    {
        public static void AddStyles(SharpKml.Dom.Document doc)
        {
            Style style = new Style();
            style.Id = "default";
            style.Icon = new IconStyle();
            style.Icon.Icon = new IconStyle.IconLink(new Uri("https://maps.google.com/mapfiles/kml/paddle/blu-blank.png"));
            style.Icon.Scale = 1.0;
            style.Icon.Hotspot = new Hotspot();
            style.Icon.Hotspot.X = 32.0;
            style.Icon.Hotspot.Y = 1.0;
            style.Icon.Hotspot.XUnits = Unit.Pixel;
            style.Icon.Hotspot.YUnits = Unit.Pixel;
            style.Label = new LabelStyle();
            style.Label.Color = new Color32(0, 0, 0, 0);
            doc.AddStyle(style);

            style = new Style();
            style.Id = "flood";
            style.Icon = new IconStyle();
            style.Icon.Icon = new IconStyle.IconLink(new Uri("https://maps.google.com/mapfiles/kml/shapes/water.png"));
            style.Icon.Scale = 1.0;
            style.Icon.Hotspot = new Hotspot();
            style.Icon.Hotspot.X = 32.0;
            style.Icon.Hotspot.Y = 1.0;
            style.Icon.Hotspot.XUnits = Unit.Pixel;
            style.Icon.Hotspot.YUnits = Unit.Pixel;
            style.Label = new LabelStyle();
            style.Label.Color = new Color32(0, 0, 0, 0);
            doc.AddStyle(style);

            style = new Style();
            style.Id = "fog";
            style.Icon = new IconStyle();
            style.Icon.Icon = new IconStyle.IconLink(new Uri("https://maps.google.com/mapfiles/kml/shapes/shaded_dot.png"));
            style.Icon.Scale = 1.0;
            style.Icon.Hotspot = new Hotspot();
            style.Icon.Hotspot.X = 32.0;
            style.Icon.Hotspot.Y = 1.0;
            style.Icon.Hotspot.XUnits = Unit.Pixel;
            style.Icon.Hotspot.YUnits = Unit.Pixel;
            style.Label = new LabelStyle();
            style.Label.Color = new Color32(0, 0, 0, 0);
            doc.AddStyle(style);

            style = new Style();
            style.Id = "rain";
            style.Icon = new IconStyle();
            style.Icon.Icon = new IconStyle.IconLink(new Uri("https://maps.google.com/mapfiles/kml/shapes/rainy.png"));
            style.Icon.Scale = 1.0;
            style.Icon.Hotspot = new Hotspot();
            style.Icon.Hotspot.X = 32.0;
            style.Icon.Hotspot.Y = 1.0;
            style.Icon.Hotspot.XUnits = Unit.Pixel;
            style.Icon.Hotspot.YUnits = Unit.Pixel;
            style.Label = new LabelStyle();
            style.Label.Color = new Color32(0, 0, 0, 0);
            doc.AddStyle(style);
        }

        public static Uri GetStyleId(StyleSource styleSource, string keyValue)
        {
            string output = "default";

            switch (styleSource)
            {
                case StyleSource.mPingJson:
                    switch (keyValue)
                    {
                        case "3":
                        case "5":
                            output = "rain";
                            break;
                        case "13":
                            output = "hail";
                            break;
                        case "40":
                            output = "flood";
                            break;
                        case "45":
                            output = "fog";
                            break;
                        default:
                            break;
                    }
                    break;
                case StyleSource.WazeJson:
                    break;
                default:
                    break;
            }

            return new Uri($"#{output}", UriKind.Relative);
        }

        public enum StyleSource
        {
            mPingJson,
            WazeJson
        }
    }
}
