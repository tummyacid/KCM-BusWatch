using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace MetroMonitor
{
    public class MetroRequest
    {
        public string version { get; set; }
        public string method { get; set; }
        [JsonPropertyName("params")]
        public Params _params { get; set; }
    }

    public class Params
    {
        public Linesrequest LinesRequest { get; set; }
    }

    public class Linesrequest
    {
        public string Radius { get; set; }
        public string GetStopTimes { get; set; }
        public string GetStopTripInfo { get; set; }
        public int NumStopTimes { get; set; }
        public string SuppressLinesUnloadOnly { get; set; }
        public string Client { get; set; }
        public string StopId { get; set; }
        public int NumTimesPerLine { get; set; }
    }

}
