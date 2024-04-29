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
            string iconHost = "https://joshuacarroll.github.io/GeoTransformer/icons";

            doc.AddStyle(CreateStyle("default", $"{iconHost}/caraccident.png"));
            doc.AddStyle(CreateStyle("damage-green", $"{iconHost}/damage-green.png"));
            doc.AddStyle(CreateStyle("damage-amber", $"{iconHost}/damage-amber.png"));
            doc.AddStyle(CreateStyle("damage-red", $"{iconHost}/damage-red.png"));
            doc.AddStyle(CreateStyle("default", $"{iconHost}/default.png"));
            doc.AddStyle(CreateStyle("flood", $"{iconHost}/flood.png"));
            doc.AddStyle(CreateStyle("fog", $"{iconHost}/fog.png"));
            doc.AddStyle(CreateStyle("freezingrain", $"{iconHost}/freezingrain.png"));
            doc.AddStyle(CreateStyle("hail-green", $"{iconHost}/hail-green.png"));
            doc.AddStyle(CreateStyle("hail-amber", $"{iconHost}/hail-amber.png"));
            doc.AddStyle(CreateStyle("hail-red", $"{iconHost}/hail-red.png"));
            doc.AddStyle(CreateStyle("rain", $"{iconHost}/rain.png"));
            doc.AddStyle(CreateStyle("snow", $"{iconHost}/snow.png"));
            doc.AddStyle(CreateStyle("tornado", $"{iconHost}/tornado.png"));
            doc.AddStyle(CreateStyle("waterspout", $"{iconHost}/waterspout.png"));
        }

        public static Uri GetStyleId(StyleSource styleSource, string keyValue)
        {
            string output = "default";

            switch (styleSource)
            {
                case StyleSource.mPingJson:
                    switch (keyValue)
                    {
                        case "1": // Test
                        case "2": // None (NULL)
                            output = "default";
                            break;
                        case "3": // Rain/Snow (Rain)
                        case "4": // Rain/Snow (Freezing Rain)
                        case "5": // Rain/Snow (Drizzle)
                        case "6": // Rain/Snow (Freezing Drizzle)
                            output = "rain";
                            break;
                        case "7": // Rain/Snow (Ice Pellets/Sleet)
                        case "8": // Rain/Snow (Snow and/or Graupel)
                        case "9": // Rain/Snow (Mixed Rain and Snow)
                        case "10": // Rain/Snow (Mixed Ice Pellets and Snow)
                        case "48": // Rain/Snow (Mixed Freezing Rain and Ice Pellets)
                        case "11": // Rain/Snow (Mixed Rain and Ice Pellets)
                        case "12": // Rain/Snow (Graupel)
                            output = "snow";
                            break;
                        case "13": // Hail (Pea (0.25 in.))
                        case "14": // Hail (Half-inch (0.50 in.))
                        case "15": // Hail (Dime (0.75 in.))
                            output = "hail-green";
                            break;
                        case "16": // Hail (Quarter (1.00 in.))
                        case "17": // Hail (Half Dollar (1.25 in.))
                        case "18": // Hail (Ping Pong Ball (1.50 in.))
                        case "19": // Hail (Golf Ball (1.75 in.))
                            output = "hail-amber";
                            break;
                        case "20": // Hail (Hen Egg (2.00 in.))
                        case "21": // Hail (Hen Egg+ (2.25 in.))
                        case "22": // Hail (Tennis Ball (2.50 in.))
                        case "23": // Hail (Baseball (2.75 in.))
                        case "24": // Hail (Tea Cup (3.00 in.))
                        case "25": // Hail (Baseball+ (3.25 in.))
                        case "26": // Hail (Baseball++ (3.50 in.))
                        case "27": // Hail (Grapefruit- (3.75 in.))
                        case "28": // Hail (Grapefruit (4.00 in.))
                        case "29": // Hail (Grapefruit+ (4.25 in.))
                        case "30": // Hail (Softball (4.50 in.))
                        case "31": // Hail (Softball+ (4.75 in.))
                        case "32": // Hail (Softball++ (>=5.00 in.))
                            output = "hail-red";
                            break;
                        case "33": // Wind Damage (Lawn furniture or trash cans displaced; Small twigs broken)
                        case "34": // Wind Damage (1-inch tree limbs broken; Shingles blown off)
                        case "35": // Wind Damage (3-inch tree limbs broken; Power poles broken)
                            output = "damage-green";
                            break;
                        case "36": // Wind Damage (Trees uprooted or snapped; Roof blown off)
                            output = "damage-amber";
                            break;
                        case "37": // Wind Damage (Homes/Buildings completely destroyed)
                            output = "damage-red";
                            break;
                        case "38": // Tornado (Tornado (on ground))
                            output = "tornado";
                            break;
                        case "39": // Tornado (Water Spout)
                            output = "waterspout";
                            break;
                        case "40": // Flood (River/Creek overflowing; Cropland/Yard/Basement Flooding)
                        case "41": // Flood (Street/road flooding; Street/road closed; Vehicles stranded)
                        case "42": // Flood (Homes or buildings filled with water)
                        case "43": // Flood (Homes, buildings or vehicles swept away)
                        case "44": // Mudslide
                            output = "flood";
                            break;
                        case "45": // Reduced Visibility (Dense Fog)
                        case "46": // Reduced Visibility (Blowing Dust/Sand)
                        case "47": // Reduced Visibility (Blowing Snow)
                        case "49": // Reduced Visibility (Snow Squall)
                        case "60": // Reduced Visibility (Smoke)
                            output = "fog";
                            break;
                        case "50": // Winter Weather Impacts (Downed tree limbs or power lines from snow or ice)
                        case "51": // Winter Weather Impacts (Frozen or burst water pipes)
                        case "52": // Winter Weather Impacts (Roof or structural collapse from snow or ice)
                        case "53": // Winter Weather Impacts (School or business delay or early dismissal)
                        case "54": // Winter Weather Impacts (School or business closure)
                        case "55": // Winter Weather Impacts (Power or internet outage or disruption)
                        case "56": // Winter Weather Impacts (Road closure)
                        case "57": // Winter Weather Impacts (Icy sidewalks, driveways, and/or parking lots)
                        case "58": // Winter Weather Impacts (Snow accumulating only on grass)
                        case "59": // Winter Weather Impacts (Snow accumulating on roads and sidewalks)
                            output = "snow";
                            break;
                        case "61": // Groundhog (Shadow)
                        case "62": // Groundhog (No Shadow)
                        default:
                            output = "default";
                            break;
                    }
                    break;
                case StyleSource.WazeJson:
                    Enum.TryParse(keyValue, out WazeJson.AlertTypes alertType);

                    switch (alertType) {
                        case WazeJson.AlertTypes.ACCIDENT_ACCIDENT_MAJOR:
                        case WazeJson.AlertTypes.ACCIDENT_ACCIDENT_MINOR:
                        case WazeJson.AlertTypes.ACCIDENT_NO_SUBTYPE:
                            output = "caraccident.png";
                            break;
                        case WazeJson.AlertTypes.JAM_JAM_MODERATE_TRAFFIC:
                            output = "";
                            break;
                        case WazeJson.AlertTypes.JAM_JAM_HEAVY_TRAFFIC:
                            output = "";
                            break;
                        case WazeJson.AlertTypes.JAM_JAM_STAND_STILL_TRAFFIC:
                            output = "";
                            break;
                        case WazeJson.AlertTypes.JAM_JAM_LIGHT_TRAFFIC:
                            output = "";
                            break;
                        case WazeJson.AlertTypes.JAM_NO_SUBTYPE:
                            output = "";
                            break;
                        case WazeJson.AlertTypes.WEATHERHAZARD_HAZARD_HAZARD_ON_ROAD:
                            output = "";
                            break;
                        case WazeJson.AlertTypes.WEATHERHAZARD_HAZARD_HAZARD_ON_SHOULDER:
                            output = "";
                            break;
                        case WazeJson.AlertTypes.WEATHERHAZARD_HAZARD_HAZARD_WEATHER:
                            output = "";
                            break;
                        case WazeJson.AlertTypes.WEATHERHAZARD_HAZARD_HAZARD_ON_ROAD_OBJECT:
                            output = "";
                            break;
                        case WazeJson.AlertTypes.WEATHERHAZARD_HAZARD_HAZARD_ON_ROAD_POT_HOLE:
                            output = "";
                            break;
                        case WazeJson.AlertTypes.WEATHERHAZARD_HAZARD_HAZARD_ON_ROAD_ROAD_KILL:
                            output = "";
                            break;
                        case WazeJson.AlertTypes.WEATHERHAZARD_HAZARD_HAZARD_ON_SHOULDER_CAR_STOPPED:
                            output = "";
                            break;
                        case WazeJson.AlertTypes.WEATHERHAZARD_HAZARD_HAZARD_ON_SHOULDER_ANIMALS:
                            output = "";
                            break;
                        case WazeJson.AlertTypes.WEATHERHAZARD_HAZARD_HAZARD_ON_SHOULDER_MISSING_SIGN:
                            output = "";
                            break;
                        case WazeJson.AlertTypes.WEATHERHAZARD_HAZARD_HAZARD_WEATHER_FOG:
                            output = "";
                            break;
                        case WazeJson.AlertTypes.WEATHERHAZARD_HAZARD_HAZARD_WEATHER_HAIL:
                            output = "";
                            break;
                        case WazeJson.AlertTypes.WEATHERHAZARD_HAZARD_HAZARD_WEATHER_HEAVY_RAIN:
                            output = "";
                            break;
                        case WazeJson.AlertTypes.WEATHERHAZARD_HAZARD_HAZARD_WEATHER_HEAVY_SNOW:
                            output = "";
                            break;
                        case WazeJson.AlertTypes.WEATHERHAZARD_HAZARD_HAZARD_WEATHER_FLOOD:
                            output = "";
                            break;
                        case WazeJson.AlertTypes.WEATHERHAZARD_HAZARD_HAZARD_WEATHER_MONSOON:
                            output = "";
                            break;
                        case WazeJson.AlertTypes.WEATHERHAZARD_HAZARD_HAZARD_WEATHER_TORNADO:
                            output = "";
                            break;
                        case WazeJson.AlertTypes.WEATHERHAZARD_HAZARD_HAZARD_WEATHER_HEAT_WAVE:
                            output = "";
                            break;
                        case WazeJson.AlertTypes.WEATHERHAZARD_HAZARD_HAZARD_WEATHER_HURRICANE:
                            output = "";
                            break;
                        case WazeJson.AlertTypes.WEATHERHAZARD_HAZARD_HAZARD_WEATHER_FREEZING_RAIN:
                            output = "";
                            break;
                        case WazeJson.AlertTypes.WEATHERHAZARD_HAZARD_HAZARD_ON_ROAD_LANE_CLOSED:
                            output = "";
                            break;
                        case WazeJson.AlertTypes.WEATHERHAZARD_HAZARD_HAZARD_ON_ROAD_OIL:
                            output = "";
                            break;
                        case WazeJson.AlertTypes.WEATHERHAZARD_HAZARD_HAZARD_ON_ROAD_ICE:
                            output = "";
                            break;
                        case WazeJson.AlertTypes.WEATHERHAZARD_HAZARD_HAZARD_ON_ROAD_CONSTRUCTION:
                            output = "";
                            break;
                        case WazeJson.AlertTypes.WEATHERHAZARD_HAZARD_HAZARD_ON_ROAD_CAR_STOPPED:
                            output = "";
                            break;
                        case WazeJson.AlertTypes.WEATHERHAZARD_HAZARD_HAZARD_ON_ROAD_TRAFFIC_LIGHT_FAULT:
                            output = "";
                            break;
                        case WazeJson.AlertTypes.WEATHERHAZARD_HAZARD_NO_SUBTYPE:
                            output = "";
                            break;
                        case WazeJson.AlertTypes.MISC_NO_SUBTYPE:
                            output = "";
                            break;
                        case WazeJson.AlertTypes.CONSTRUCTION_NO_SUBTYPE:
                            output = "";
                            break;
                        case WazeJson.AlertTypes.ROAD_CLOSED_ROAD_CLOSED_HAZARD:
                            output = "";
                            break;
                        case WazeJson.AlertTypes.ROAD_CLOSED_ROAD_CLOSED_CONSTRUCTION:
                            output = "";
                            break;
                        case WazeJson.AlertTypes.ROAD_CLOSED_ROAD_CLOSED_EVENT:
                            output = "";
                            break;
                        case WazeJson.AlertTypes.ROAD_CLOSED_NO_SUBTYPE:
                            output = "";
                            break;
                        default:
                            output = "default";
                            break;
                    }
                    
                    break;
                default:
                    break;
            }

            return new Uri($"#{output}", UriKind.Relative);
        }

        public static SharpKml.Dom.Style CreateStyle(string styleId, string iconUri) {
            Style style = new Style();
            style.Id = styleId;
            style.Icon = new IconStyle();
            style.Icon.Icon = new IconStyle.IconLink(new Uri(iconUri));
            style.Icon.Scale = 1.0;
            style.Icon.Hotspot = new Hotspot();
            style.Icon.Hotspot.X = 32.0;
            style.Icon.Hotspot.Y = 1.0;
            style.Icon.Hotspot.XUnits = Unit.Pixel;
            style.Icon.Hotspot.YUnits = Unit.Pixel;
            style.Label = new LabelStyle();
            style.Label.Color = new Color32(0, 0, 0, 0);
            return style;
        }

        public enum StyleSource
        {
            mPingJson,
            WazeJson
        }
    }
}
