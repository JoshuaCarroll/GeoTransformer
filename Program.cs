using System.Net.Http.Headers;
using System.Net.Mime;
using System.Reflection.PortableExecutable;
using System.Text;

namespace GeoTransformer
{
    internal class Program
    {
        string url = string.Empty;
        string data = string.Empty;

        static void Main(string[] args)
        {
            HttpServer http = new HttpServer("127.0.0.1", 8080);
            http.Start();
        }
    }
}
