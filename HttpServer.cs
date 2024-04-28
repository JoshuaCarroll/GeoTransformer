using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using SharpKml.Dom;
using Newtonsoft.Json;
using SharpKml.Base;
using System.Reflection.PortableExecutable;

namespace GeoTransformer
{
    public class HttpServer
    {
        private readonly IPAddress ipAddress;
        private readonly int port;
        private readonly TcpListener serverListenter;

        private DateTime lastConsoleLog;
        private System.TimeSpan showLogTimeThisOften = new System.TimeSpan(0, 0, 2);

        public HttpServer(string ipAddress, int port)
        {
            this.ipAddress = IPAddress.Parse(ipAddress);
            this.port = port;

            this.serverListenter = new TcpListener(this.ipAddress, port);
        }

        public void Start()
        {
            ConsoleWriteLine($"Server started on port {port}.");
            ConsoleWriteLine("Listening for requests...");

            while (true)
            {
                this.serverListenter.Start();

                TcpClient connection = serverListenter.AcceptTcpClient();
                NetworkStream stream = connection.GetStream();

                Byte[] buffer = new Byte[4096];
                Int32 bytes = stream.Read(buffer, 0, buffer.Length);
                string request = System.Text.Encoding.ASCII.GetString(buffer, 0, bytes);

                if (request != string.Empty)
                {
                    string[] requestArray = request.Split(Environment.NewLine);

                    if (requestArray[0] != "GET /favicon.ico HTTP/1.1")
                    {
                        string requestedUrl = requestArray[0].Substring(6).Split([' '])[0];

                        string contentType = "text/plain";
                        if (request.Split(Environment.NewLine)[2].StartsWith("User-Agent: GoogleEarth"))
                        {
                            contentType = "application/vnd.google-earth.kml+xml";
                        }

                        string response = HandleHttpRequest(requestedUrl);

                        WriteResponse(stream, response, contentType);
                    }
                }
                connection.Close();
            }
        }

        private string HandleHttpRequest(string requestedUrl)
        {
            using (HttpClient client = new HttpClient())
            {
                string output = string.Empty;
                client.Timeout = System.TimeSpan.FromSeconds(180);

                try
                {
                    Uri uri = new Uri(requestedUrl);

                    ConsoleWriteLine("Recieved request for data from " + uri.Host);

                    HttpRequestMessage requestMessage = new HttpRequestMessage
                    {
                        Method = HttpMethod.Get,
                        RequestUri = uri,
                    };

                    if (Uri.UnescapeDataString(uri.AbsoluteUri).Contains('|'))
                    {
                        string[] queryStringParams = Uri.UnescapeDataString(uri.AbsoluteUri).Split(['|', '~']);

                        for (int i = 1; i < queryStringParams.Length; i=i+2)
                        {
                            requestMessage.Headers.Add(queryStringParams[i], queryStringParams[i + 1]);
                            ConsoleWriteLine($"  - Adding header: {queryStringParams[i]}={queryStringParams[i + 1]}", ConsoleColor.Green, ConsoleColor.DarkGray);
                        }

                        requestMessage.RequestUri = new Uri(queryStringParams[0]);
                    }
                    requestMessage.Headers.Add("User-Agent", "Mozilla/5.0");

                    if (uri.Host == "mping.ou.edu")
                    {
                        string qs = requestMessage.RequestUri.AbsoluteUri.Contains('?') ? "&" : "?";
                        System.TimeSpan ageOfOldestReport = System.TimeSpan.FromDays(1);
                        DateTime dt = DateTime.UtcNow.Subtract(ageOfOldestReport);
                        requestMessage.RequestUri = new Uri(requestMessage.RequestUri.AbsoluteUri + qs + "obtime_gte=" + dt.ToString("yyyy-MM-dd HH:mm:ss"));  // 2012-02-20 03:00:00
                    }

                    ConsoleWriteLine("Sending request:" + Environment.NewLine + requestMessage);
                    HttpResponseMessage httpResponse = client.Send(requestMessage);
                    
                    httpResponse.EnsureSuccessStatusCode();
                    ConsoleWriteLine("  - Recieving response", ConsoleColor.Green, ConsoleColor.DarkGray);

                    Stream streamResponse = httpResponse.Content.ReadAsStream();
                    StreamReader streamReader = new StreamReader(streamResponse);
                    string responseText = streamReader.ReadToEnd();
                    ConsoleWriteLine("  - Response done. Parsing objects.", ConsoleColor.Green, ConsoleColor.DarkGray);

                    switch (uri.Host)
                    {
                        case "www.waze.com":
                            output = WazeJson.FromJson(responseText).ToKml();
                            break;
                        case "mping.ou.edu":
                            output = mPingJson.FromJson(responseText).ToKml();
                            break;
                        default:
                            break;
                    }
                }
                catch (Exception ex)
                {
                    ConsoleWriteLine(ex.ToString(), ConsoleColor.DarkRed, ConsoleColor.Red);
                }

                return output;
            }
        }

        private void WriteResponse(NetworkStream networkStream, string message, string contentType)
        {
            var contentLength = Encoding.UTF8.GetByteCount(message);

            ////
            var response = $@"HTTP/1.1 200 OK
Content-Type: {contentType}; charset=UTF-8
Content-Length: {contentLength}

{message}";
            var responseBytes = Encoding.UTF8.GetBytes(response);

            networkStream.Write(responseBytes, 0, responseBytes.Length);

            int lengthToDisplay = 400;
            if (response.Length < lengthToDisplay)
            {
                ConsoleWriteLine(response, ConsoleColor.Blue);
            }
            else
            {
                ConsoleWriteLine(response.Substring(0, lengthToDisplay) + "...", ConsoleColor.Blue);
            }
        }

        private void ConsoleWriteLine(string msg, ConsoleColor timeColor = ConsoleColor.Green, ConsoleColor messageColor = ConsoleColor.Gray) 
        {
            DateTime logTimeStamp = System.DateTime.Now.ToLocalTime();

            if (logTimeStamp.Subtract(lastConsoleLog) > showLogTimeThisOften)
            {
                Console.ForegroundColor = timeColor;
                Console.WriteLine(System.DateTime.Now.ToLocalTime());
                lastConsoleLog = logTimeStamp;
            }
            Console.ForegroundColor = messageColor;
            Console.WriteLine(msg);
        }
    }
}
