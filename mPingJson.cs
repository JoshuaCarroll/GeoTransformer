using ExtensionMethods;
using Newtonsoft.Json;
using SharpKml.Base;
using SharpKml.Dom;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeoTransformer
{
    public class mPingJson
    {
        public static mPingJson FromJson(string jsonData) {
            mPingJson mpingjson = JsonConvert.DeserializeObject<mPingJson>(jsonData);
            return mpingjson;
        }

        public string ToKml()
        {
            Kml kml = new Kml();
            var doc = new SharpKml.Dom.Document { Name = "Map items", Open = false };
            kml.Feature = doc;
            KmlStyles.AddStyles(doc);

            foreach (Result result in results.EmptyIfNull())
            {
                if (result.description != "NULL" && result.geom.coordinates[0] < 1)
                {
                    Placemark p = new Placemark
                    {
                        GXBalloonVisibility = false,
                        Geometry = new Point
                        {
                            Coordinate = new SharpKml.Base.Vector(result.geom.coordinates[1], result.geom.coordinates[0]),
                        }
                    };
                    p.Name = result.description;
                    p.Description = new Description();
                    p.Description.Text = $"<![CDATA[ <p>Category: {result.category}<br>Observed at {result.obtime}</p>Description ID: {result.description_id} ]]>";
                    p.StyleUrl = KmlStyles.GetStyleId(KmlStyles.StyleSource.mPingJson, result.description_id.ToString());
                    doc.AddFeature(p);

                    Debug.WriteLine(result.description_id + " = " + result.description);
                }
            }

            Serializer serializer = new Serializer();
            serializer.Serialize(kml);
            return serializer.Xml;
        }

        public int count { get; set; }
        public object? next { get; set; }
        public object? previous { get; set; }
        public List<Result>? results { get; set; }
    }

    public class Geom
    {
        public string? type { get; set; }
        public List<double>? coordinates { get; set; }
    }

    public class Result
    {
        public int id { get; set; }
        public DateTime obtime { get; set; }
        public string category { get; set; }
        public string description { get; set; }
        public int description_id { get; set; }
        public Geom geom { get; set; }
    }
}
