using Newtonsoft.Json;
using SharpKml.Base;
using SharpKml.Dom;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeoTransformer
{
    public class GeoJson
    {
        public static string ToKml(string jsonData)
        {
            Kml kml = new Kml();
            var doc = new SharpKml.Dom.Document { Name = "Map items", Open = false };
            kml.Feature = doc;

            GeoJson geoJson = JsonConvert.DeserializeObject<GeoJson>(jsonData);

            foreach (Feature feature in geoJson.features)
            {
                doc.AddFeature(new Placemark
                {
                    Name = feature.properties.description,
                    Geometry = new Point
                    {
                        Coordinate = new SharpKml.Base.Vector(feature.geometry.coordinates[1], feature.geometry.coordinates[0]),
                    }
                });
            }

            Serializer serializer = new Serializer();
            serializer.Serialize(kml);
            return serializer.Xml;
        }
        public string type { get; set; }
        public List<Feature> features { get; set; }
    }
    public class Feature
    {
        public int id { get; set; }
        public string type { get; set; }
        public Geometry geometry { get; set; }
        public Properties properties { get; set; }
    }

    public class Geometry
    {
        public string type { get; set; }
        public List<double> coordinates { get; set; }
    }

    public class Properties
    {
        public DateTime obtime { get; set; }
        public string category { get; set; }
        public string description { get; set; }
        public int description_id { get; set; }
    }
}
