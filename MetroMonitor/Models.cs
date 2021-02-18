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

    public class MetroResponse
    {
        public string version { get; set; }
        public Result[] result { get; set; }
    }

    public class Result
    {
        public Stoptimeresult[] StopTimeResult { get; set; }
        public Tripresult[] TripResult { get; set; }
        public Realtimeresult[] RealTimeResults { get; set; }
        public string SystemTime { get; set; }
    }

    public class Stoptimeresult
    {
        public Line[] Lines { get; set; }
        public Stoptime[] StopTimes { get; set; }
        public object[] ServiceResumesTimes { get; set; }
        public object[] GeneralRemarks { get; set; }
        public object[] StopIdRemarks { get; set; }
        public object[] LinesRemarks { get; set; }
        public object[] TimesRemarks { get; set; }
        public Statusinforow[] StatusInfoRows { get; set; }
        public int StopNumTotal { get; set; }
        public int StopNumReturned { get; set; }
        public float StopNewRadius { get; set; }
        public Validation[] Validation { get; set; }
        public Stoptimesoutput[] StopTimesOutput { get; set; }
    }

    public class Line
    {
        public int LineDirId { get; set; }
        public int Lon { get; set; }
        public int Lat { get; set; }
        public string StopId { get; set; }
        public string StopName { get; set; }
        public string StopAbbr { get; set; }
        public string AtisStopId { get; set; }
        public string DirectionName { get; set; }
        public int LineDirIdConst { get; set; }
        public string LineName { get; set; }
        public string LineAbbr { get; set; }
        public int LineId { get; set; }
        public int LineIdConst { get; set; }
        public bool LineRemarksExist { get; set; }
        public string StopPublicId { get; set; }
        public bool StopRemarksExist { get; set; }
        public int NumericalLineAbbr { get; set; }
    }

    public class Stoptime
    {
        public string StopId { get; set; }
        public string ETime { get; set; }
        public int ETimeSPC { get; set; }
        public int Date { get; set; }
        public int ETimeUnadjusted { get; set; }
        public int DateUnadjusted { get; set; }
        public int TripId { get; set; }
        public int BlockId { get; set; }
        public int TrapezeBlock { get; set; }
        public int BlockNum { get; set; }
        public int LineGroup { get; set; }
        public int ServiceGroupId { get; set; }
        public int StopFlag { get; set; }
        public int StopTimesIndex { get; set; }
        public string DestinationSign { get; set; }
        public string LastTripOfDayLineDir { get; set; }
        public string LastTripOfDayPattern { get; set; }
        public int LineDirId { get; set; }
        public int PatternId { get; set; }
        public string StopNum { get; set; }
        public int NodeId { get; set; }
        public string NodeName { get; set; }
        public string LineName { get; set; }
        public string LineAbbr { get; set; }
        public string DirectionName { get; set; }
        public Realtime RealTime { get; set; }
        public Serviceadjustmenttype[] ServiceAdjustmentTypes { get; set; }
        public string StopPublicId { get; set; }
    }

    public class Realtime
    {
        public string Valid { get; set; }
        public string Reliable { get; set; }
        public string Estimatedtime { get; set; }
        public string Estimatedminutes { get; set; }
        public string Estindicator { get; set; }
        public string Polltime { get; set; }
        public string Adherence { get; set; }
        public string Trend { get; set; }
        public string Speed { get; set; }
        public string Stopped { get; set; }
        public string Vehicleid { get; set; }
        public string Lat { get; set; }
        public string Long { get; set; }
        public string Bustimedist { get; set; }
        public string Bustimenote { get; set; }
    }

    public class Serviceadjustmenttype
    {
        public int ServiceAdjustmentType { get; set; }
    }

    public class Statusinforow
    {
        public string StatusInfo { get; set; }
        public string StopId { get; set; }
    }

    public class Validation
    {
        public string Name { get; set; }
        public int Instance { get; set; }
        public string Type { get; set; }
        public string Code { get; set; }
        public string Message { get; set; }
        public string Action { get; set; }
        public object[] Extra { get; set; }
    }

    public class Stoptimesoutput
    {
        public string StopId { get; set; }
        public string ETime { get; set; }
        public int ETimeSPC { get; set; }
        public int Date { get; set; }
        public int ETimeUnadjusted { get; set; }
        public int DateUnadjusted { get; set; }
        public int TripId { get; set; }
        public int BlockId { get; set; }
        public int TrapezeBlock { get; set; }
        public int BlockNum { get; set; }
        public int LineGroup { get; set; }
        public int ServiceGroupId { get; set; }
        public int StopFlag { get; set; }
        public int StopTimesIndex { get; set; }
        public string DestinationSign { get; set; }
        public string LastTripOfDayLineDir { get; set; }
        public string LastTripOfDayPattern { get; set; }
        public int LineDirId { get; set; }
        public int PatternId { get; set; }
        public string StopNum { get; set; }
        public int NodeId { get; set; }
        public string NodeName { get; set; }
        public string LineName { get; set; }
        public string LineAbbr { get; set; }
        public string DirectionName { get; set; }
        public Serviceadjustmenttype1[] ServiceAdjustmentTypes { get; set; }
        public string StopPublicId { get; set; }
        public int RealTime { get; set; }
        public int RealTimeSPC { get; set; }
        public bool IgnoreAdherence { get; set; }
        public int Lon { get; set; }
        public int Lat { get; set; }
        public string VehicleNumber { get; set; }
        public int EstimatedScheduleAdherence { get; set; }
        public string TimeOutput { get; set; }
    }

    public class Serviceadjustmenttype1
    {
        public int ServiceAdjustmentType { get; set; }
    }

    public class Tripresult
    {
        public object[] Trips { get; set; }
    }

    public class Realtimeresult
    {
        public int RealTime { get; set; }
        public int RealTimeSPC { get; set; }
        public int RealTimeMPC { get; set; }
        public int ETime { get; set; }
        public int ETimeSPC { get; set; }
        public int StopId { get; set; }
        public int TripId { get; set; }
        public int BlockId { get; set; }
        public int BayNum { get; set; }
        public int EstimatedScheduleAdherence { get; set; }
        public int LastTimingPointId { get; set; }
        public string LastTimingPointName { get; set; }
        public int LastTimingPointScheduleTime { get; set; }
        public bool LastUpdateThreshold { get; set; }
        public bool AdherenceThreshold { get; set; }
        public bool IgnoreAdherence { get; set; }
        public int LineDirId { get; set; }
        public int Lon { get; set; }
        public int Lat { get; set; }
        public string VehicleNumber { get; set; }
    }

}
