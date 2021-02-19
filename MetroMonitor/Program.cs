using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text.Json;
using System.Threading;

namespace MetroMonitor
{
    class Program
    {
        private const int gpsMovementThreashold = 500;
        private static TimeSpan pollingInterval = new TimeSpan(0, 0, 30);
        static System.Collections.Generic.Dictionary<int, System.Drawing.Point> history = new System.Collections.Generic.Dictionary<int, System.Drawing.Point>();
        static void Main(string[] args)
        {
            while (true)
            {
                var busState = checkBusStop("1000");
                Console.WriteLine($"{DateTime.Now}");
                Alarm(busState);
                Thread.Sleep(Convert.ToInt32(pollingInterval.TotalMilliseconds));
            }
        }

        private static MetroResponse checkBusStop(String aStopId)
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
                        StopId = aStopId,
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
            hr.Close();

            return resp;
        }

        /// <summary>
        /// Check the state of the bus response for any alarm conditions. 
        /// </summary>
        /// <param name="busState">MetroResponse from the KCM api</param>
        /// <returns>-1 if no alarm states present, otherwise it returns a list of bus Ids that satisfy an alarm state</returns>
        public static String[] Alarm(MetroResponse busState)
        {
            List<String> movingBuses = new List<String>();

            foreach (var aBus in busState.result.First().RealTimeResults.ToArray())
            {
                System.Drawing.Point busPosition = new System.Drawing.Point(aBus.Lon, aBus.Lat);
                if (!history.ContainsKey(aBus.TripId))
                    history.Add(aBus.TripId, busPosition);
                else
                {
                    if (getDistance(history[aBus.TripId], busPosition) > gpsMovementThreashold)
                    {
                        movingBuses.Add(aBus.VehicleNumber);
                        Console.WriteLine($"{aBus.VehicleNumber} {getDistance(history[aBus.TripId], busPosition)} away");
                    }
                    history[aBus.TripId] = new System.Drawing.Point(aBus.Lon, aBus.Lat);
                }
            }
            return movingBuses.ToArray();
        }

        public static long getDistance(System.Drawing.Point location1, System.Drawing.Point location2)
        {
            long deltaX = location1.X - location2.X;
            long deltaY = location1.Y - location2.Y;

            double answer = Math.Sqrt(deltaX * deltaX + deltaY * deltaY);

            return Convert.ToInt32(answer);
        }
    }
}
