using System;
using System.IO;
using System.Net;
using System.Text.Json;

namespace MetroMonitor
{
    class Program
    {
        static void Main(string[] args)
        {
            HttpWebRequest wr = (HttpWebRequest)HttpWebRequest.Create(@"https://tripplanner.kingcounty.gov/InfoWeb");
            wr.ContentType = "application/json; charset=utf-8";
            wr.Method = "POST";

            MetroRequest req = new MetroRequest
            {
                method = "GetBusTimes",
                version = "1.1",
                _params = new Params
                {
                    LinesRequest = new Linesrequest
                    {
                        Client = "MobileWeb",
                        GetStopTimes = "1",
                        GetStopTripInfo = "1",
                        NumStopTimes = 150,
                        NumTimesPerLine = 10,
                        Radius = "0",
                        StopId = "1000",
                        SuppressLinesUnloadOnly = "1"
                    }
                }
            };

            using (var streamWriter = new StreamWriter(wr.GetRequestStream()))
            {
                string json = JsonSerializer.Serialize(req);

                streamWriter.Write(json);
                streamWriter.Flush();
            }

            var hr = (HttpWebResponse)wr.GetResponse();
            using (var streamReader = new StreamReader(hr.GetResponseStream()))
            {
                var responseText = streamReader.ReadToEnd();
                Console.WriteLine(responseText);
            }
            hr.Close();
        }
    }
}
