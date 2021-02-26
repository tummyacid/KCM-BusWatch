using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Text.Json;
using System.Threading;

namespace MetroMonitor
{
    class Program
    {
        static System.Collections.Generic.Dictionary<String, System.Drawing.Point> busInHolding = new System.Collections.Generic.Dictionary<String, System.Drawing.Point>();
        static System.Collections.Generic.Dictionary<String, System.Drawing.Point> busApproaching = new System.Collections.Generic.Dictionary<String, System.Drawing.Point>();

        static void Main(string[] args)
        {

            while (true)
            {
                var state = getBusState("9999");

                EvaluateAndPrint(state);
                Thread.Sleep(30000);
            }
        }

        private static MetroResponse getBusState(String StopId)
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
                        StopId = StopId,
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
        /// <returns>This routine only writes to console</returns>
        public static void EvaluateAndPrint(MetroResponse busState)
        {
            int latFenceN = 47000000; //it has left the staging area and started southbound
            int latFenceS = 47000000; //too late i missed it

            foreach (var aBus in busState.result.First().RealTimeResults.ToArray())
            {
                if (latFenceN < aBus.Lat)
                {
                    if (busInHolding.ContainsKey(aBus.VehicleNumber))
                    {
                        busInHolding[aBus.VehicleNumber] = new System.Drawing.Point(aBus.Lon, aBus.Lat);
                    }
                    else
                    {
                        busInHolding.Add(aBus.VehicleNumber, new System.Drawing.Point(aBus.Lon, aBus.Lat));
                    }
                }
                else if (latFenceS < aBus.Lat)
                {
                    if (busInHolding.ContainsKey(aBus.VehicleNumber))
                    {
                        busInHolding.Remove(aBus.VehicleNumber);
                    }

                    if (busApproaching.ContainsKey(aBus.VehicleNumber))
                    {
                        busApproaching[aBus.VehicleNumber] = new System.Drawing.Point(aBus.Lon, aBus.Lat);
                    }
                    else
                    {
                        busApproaching.Add(aBus.VehicleNumber, new System.Drawing.Point(aBus.Lon, aBus.Lat));
                    }
                }
                else
                {
                    if (busInHolding.ContainsKey(aBus.VehicleNumber))
                    {
                        busInHolding.Remove(aBus.VehicleNumber);
                    }
                    if (busApproaching.ContainsKey(aBus.VehicleNumber))
                    {
                        busApproaching.Remove(aBus.VehicleNumber);
                    }
                }
            }

            Console.WriteLine($"{busInHolding.Count()} in staging area");
            foreach(var x in busApproaching.Keys)
                Console.WriteLine($"{x} \t {getDistance(busApproaching[x], new System.Drawing.Point(busApproaching[x].X, latFenceS)) }\t{DateTime.Now}");

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
