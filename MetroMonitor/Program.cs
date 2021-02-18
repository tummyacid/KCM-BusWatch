using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Text.Json;

namespace MetroMonitor
{
    class Program
    {
        static System.Drawing.Point busStop = new System.Drawing.Point(-10000000, 500000);
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

            MetroResponse resp = null;
            var hr = (HttpWebResponse)wr.GetResponse();
            using (var streamReader = new StreamReader(hr.GetResponseStream()))
            {
                resp = JsonSerializer.Deserialize<MetroResponse>(streamReader.ReadToEnd());
            }
            Console.WriteLine(Alarm(resp));
            hr.Close();
        }
        /// <summary>
        /// Check the state of the bus response for any alarm conditions. 
        /// </summary>
        /// <param name="busState">MetroResponse from the KCM api</param>
        /// <returns>-1 if no alarm states present, otherwise it returns the distance to the first bus that satisfies an alarm state</returns>
        public static long Alarm(MetroResponse busState)
        {
            int latFenceN = busStop.Y + 4000; //it has left the staging area and started moving
            int latFenceS = busStop.Y + 1000; //too late i missed it

            foreach (int currentLatitude in busState.result.First().RealTimeResults.Select(y => y.Lat).ToArray())
                if (latFenceN > currentLatitude && currentLatitude > latFenceS)
                    return getDistance(new System.Drawing.Point(busStop.X, currentLatitude));//Only tracking southbound progress; ignore X component by passing the busStop's X component so they difference to 0

            return -1;
        }

        public static long getDistance(System.Drawing.Point location)
        {
            long deltaX = busStop.X - location.X;
            long deltaY = busStop.Y - location.Y;

            double answer = Math.Sqrt(deltaX * deltaX + deltaY * deltaY);

            return Convert.ToInt32(answer);
        }
    }
}
