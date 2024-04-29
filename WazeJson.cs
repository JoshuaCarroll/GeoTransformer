using ExtensionMethods;
using Newtonsoft.Json;
using SharpKml.Base;
using SharpKml.Dom;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;

namespace GeoTransformer
{
    public class WazeJson
    {
        public static WazeJson FromJson(string jsonData) {
            WazeJson wazeJson = JsonConvert.DeserializeObject<WazeJson>(jsonData);
            return wazeJson;
        }

        public string ToKml()
        {
            Kml kml = new Kml();
            var doc = new SharpKml.Dom.Document { Name = "Map items", Open = false };
            kml.Feature = doc;
            KmlStyles.AddStyles(doc);

            var fdrAlerts = new Folder { Id = "data-layers", Name = "Alerts", Open = false };
            doc.AddFeature(fdrAlerts);
            foreach (Alert alert in alerts.EmptyIfNull())
            {
                fdrAlerts.AddFeature(CreatePlacemark(alert));
            }

            var fdrIrregularities = new Folder { Id = "data-layers", Name = "Irregularities", Open = false };
            doc.AddFeature(fdrIrregularities);
            foreach (Irregularity irregularity in irregularities.EmptyIfNull())
            {
                fdrIrregularities.AddFeature(CreateLine(irregularity.line));

                foreach (Alert alert in irregularity.alerts.EmptyIfNull())
                {
                    fdrIrregularities.AddFeature(CreatePlacemark(alert));
                }
            }

            var fdrJams = new Folder { Id = "data-layers", Name = "Jams", Open = false };
            doc.AddFeature(fdrJams);
            // TODO: Implement jams (blackberry or strawberry preferably)

            Serializer serializer = new Serializer();
            serializer.Serialize(kml);
            return serializer.Xml;
        }

        private static Placemark CreateLine(List<Line> lines) {
            Placemark p = new Placemark();

            var track = new SharpKml.Dom.GX.Track();
            foreach (Line line in lines)
            {
                var vector = new Vector(line.y, line.x);
                track.AddCoordinate(vector);
            }

            p.Geometry = track;
            return p;
        }
        private static Placemark CreatePlacemark(Alert alert) {
            string strAlertType = $"{alert.type.Replace(" / ", "_")}_{alert.subtype}";

            return CreatePlacemark (
                @$"<![CDATA[ <p><strong>{alert.type.Replace('_', ' ')}</strong><br>{alert.subtype.Replace('_', ' ')}</p>
                <p>
                    {alert.street} {alert.city}<br>
                    Bearing: {alert.magvar}&deg;
                    <p>{alert.reportDescription}</p>
                    Confidence: {alert.confidence}/10<br>
                    Reliability: {alert.reliability}/10<br>
                    Report rating: {alert.reportRating}/6<br>                    

                </p> ]]>", 
                alert.location.y, 
                alert.location.x,
                strAlertType
            );
        }
        private static Placemark CreatePlacemark(string description, double latitude, double longitude, string styleId = "default") {
            Placemark p = new Placemark();

            p.Description = new Description();
            p.Description.Text = description;
            p.GXBalloonVisibility = false;
            p.Geometry = new Point
            {
                Coordinate = new SharpKml.Base.Vector(latitude, longitude),
            };
            p.StyleUrl = KmlStyles.GetStyleId(KmlStyles.StyleSource.WazeJson, styleId);

            return p;
        }

        public enum AlertTypes {
            DEFAULT,
            ACCIDENT_ACCIDENT_MINOR,
            ACCIDENT_ACCIDENT_MAJOR,
            ACCIDENT_NO_SUBTYPE,
            JAM_JAM_MODERATE_TRAFFIC,
            JAM_JAM_HEAVY_TRAFFIC,
            JAM_JAM_STAND_STILL_TRAFFIC,
            JAM_JAM_LIGHT_TRAFFIC,
            JAM_NO_SUBTYPE,
            HAZARD_HAZARD_ON_ROAD,
            HAZARD_HAZARD_ON_SHOULDER,
            WEATHERHAZARD_HAZARD_HAZARD_WEATHER,
            HAZARD_HAZARD_ON_ROAD_OBJECT,
            HAZARD_HAZARD_ON_ROAD_POT_HOLE,
            HAZARD_HAZARD_ON_ROAD_ROAD_KILL,
            HAZARD_HAZARD_ON_SHOULDER_CAR_STOPPED,
            WEATHERHAZARD_HAZARD_HAZARD_ON_SHOULDER_ANIMALS,
            HAZARD_HAZARD_ON_SHOULDER_MISSING_SIGN,
            WEATHERHAZARD_HAZARD_HAZARD_WEATHER_FOG,
            WEATHERHAZARD_HAZARD_HAZARD_WEATHER_HAIL,
            WEATHERHAZARD_HAZARD_HAZARD_WEATHER_HEAVY_RAIN,
            WEATHERHAZARD_HAZARD_HAZARD_WEATHER_HEAVY_SNOW,
            WEATHERHAZARD_HAZARD_HAZARD_WEATHER_FLOOD,
            WEATHERHAZARD_HAZARD_HAZARD_WEATHER_MONSOON,
            WEATHERHAZARD_HAZARD_HAZARD_WEATHER_TORNADO,
            WEATHERHAZARD_HAZARD_HAZARD_WEATHER_HEAT_WAVE,
            WEATHERHAZARD_HAZARD_HAZARD_WEATHER_HURRICANE,
            WEATHERHAZARD_HAZARD_HAZARD_WEATHER_FREEZING_RAIN,
            HAZARD_HAZARD_ON_ROAD_LANE_CLOSED,
            HAZARD_HAZARD_ON_ROAD_OIL,
            HAZARD_HAZARD_ON_ROAD_ICE,
            HAZARD_HAZARD_ON_ROAD_CONSTRUCTION,
            HAZARD_HAZARD_ON_ROAD_CAR_STOPPED,
            HAZARD_HAZARD_ON_ROAD_TRAFFIC_LIGHT_FAULT,
            HAZARD_NO_SUBTYPE,
            MISC_NO_SUBTYPE,
            CONSTRUCTION_NO_SUBTYPE,
            ROAD_CLOSED_ROAD_CLOSED_HAZARD,
            ROAD_CLOSED_ROAD_CLOSED_CONSTRUCTION,
            ROAD_CLOSED_ROAD_CLOSED_EVENT,
            ROAD_CLOSED_NO_SUBTYPE
        }

        public List<Alert> alerts { get; set; }
        public long endTimeMillis { get; set; }
        public List<Irregularity> irregularities { get; set; }
        public long startTimeMillis { get; set; }
        public string startTime { get; set; }
        public string endTime { get; set; }
        public List<Jam> jams { get; set; }
    }
    public class Alert
    {
        public string country { get; set; }
        public int reportRating { get; set; }
        public string reportByMunicipalityUser { get; set; }
        public int confidence { get; set; }
        public int reliability { get; set; }
        public string type { get; set; }
        public string uuid { get; set; }
        public int roadType { get; set; }
        public int magvar { get; set; }
        public string subtype { get; set; }
        public string street { get; set; }
        public Location location { get; set; }
        public object pubMillis { get; set; }
        public string city { get; set; }
        public string reportDescription { get; set; }
    }

    public class Irregularity
    {
        public string country { get; set; }
        public int nThumbsUp { get; set; }
        public string updateDate { get; set; }
        public int trend { get; set; }
        public string city { get; set; }
        public List<Line> line { get; set; }
        public object detectionDateMillis { get; set; }
        public string type { get; set; }
        public string endNode { get; set; }
        public double speed { get; set; }
        public int seconds { get; set; }
        public string street { get; set; }
        public int jamLevel { get; set; }
        public int id { get; set; }
        public int nComments { get; set; }
        public bool highway { get; set; }
        public int delaySeconds { get; set; }
        public int severity { get; set; }
        public int driversCount { get; set; }
        public int alertsCount { get; set; }
        public int length { get; set; }
        public object updateDateMillis { get; set; }
        public int nImages { get; set; }
        public List<Alert> alerts { get; set; }
        public string detectionDate { get; set; }
        public double regularSpeed { get; set; }
        public string startNode { get; set; }
    }

    public class Jam
    {
        public string country { get; set; }
        public int level { get; set; }
        public string city { get; set; }
        public List<Line> line { get; set; }
        public double speedKMH { get; set; }
        public int length { get; set; }
        public string turnType { get; set; }
        public int uuid { get; set; }
        public string endNode { get; set; }
        public double speed { get; set; }
        public List<Segment> segments { get; set; }
        public string blockingAlertUuid { get; set; }
        public int roadType { get; set; }
        public int delay { get; set; }
        public string street { get; set; }
        public int id { get; set; }
        public object pubMillis { get; set; }
        public string startNode { get; set; }
    }

    public class Line
    {
        public double x { get; set; }
        public double y { get; set; }
    }

    public class Location
    {
        public double x { get; set; }
        public double y { get; set; }
    }

    public class Segment
    {
        public int fromNode { get; set; }
        public int ID { get; set; }
        public int toNode { get; set; }
        public bool isForward { get; set; }
    }


}
