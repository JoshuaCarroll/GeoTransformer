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
        public static string ToKml(string jsonData)
        {
            Kml kml = new Kml();
            var doc = new SharpKml.Dom.Document { Name = "Map items", Open = false };
            kml.Feature = doc;

            WazeJson wazeJson = JsonConvert.DeserializeObject<WazeJson>(jsonData);

            var alerts = new Folder { Id = "data-layers", Name = "Alerts", Open = false };
            doc.AddFeature(alerts);
            var irregularities = new Folder { Id = "data-layers", Name = "Irregularities", Open = false };
            doc.AddFeature(irregularities);
            var jams = new Folder { Id = "data-layers", Name = "Jams", Open = false };
            doc.AddFeature(jams);
            
            foreach (Alert alert in wazeJson.alerts)
            {
                alerts.AddFeature(CreatePlacemark(alert));
            }
            foreach (Irregularity irregularity in wazeJson.irregularities)
            {
                irregularities.AddFeature(CreateLine(irregularity.line));

                foreach (Alert alert in irregularity.alerts)
                {
                    irregularities.AddFeature(CreatePlacemark(alert));
                }
            }

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
            alert.subtype = alert.subtype.Replace('_', ' ');
            return CreatePlacemark (
                @$"<p><strong>{alert.type}</strong><br>{alert.subtype}</p>
                <p>
                    {alert.city}<br>
                    Confidence: {alert.confidence}/10<br>
                    Reliability: {alert.reliability}/10<br>

                </p>", 
                alert.location.y, 
                alert.location.x
            );
        }
        private static Placemark CreatePlacemark(string description, double latitude, double longitude) {
            Placemark p = new Placemark();

            p.Description = new Description();
            p.Description.Text = description;
            p.GXBalloonVisibility = false;
            p.Geometry = new Point
            {
                Coordinate = new SharpKml.Base.Vector(latitude, longitude),
            };

            return p;
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
