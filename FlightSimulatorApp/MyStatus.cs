
namespace FlightSimulatorApp
{
    class MyStatus
    {
        // All the possible statuses of this program.
        static public string OkStatus = "OK";
        static public string ConnectedStatus = "Connected";
        static public string NotConnectedStatus = "Not Connected";
        static public string TryingToConnectStatus = "Trying To Connect...";
        static public string DisconnectedStatus = "Disconnected From Simulator";
        static public string ConnectionFailedStatus = "Failed Connecting To Simulator";
        static public string SimulatorDisconnectedStatus = "The Simulator Disconnected";
        static public string RcvErrorStatus = "Error Trying To Receive Data From Simulator";
        static public string SendErrorStatus = "Error Trying To Send Data To Simulator";
        static public string LatitudeErrorStatus = "Received Invalid Latitude Value";
        static public string LongitudeErrorStatus = "Received Invalid Longitude Value";
        static public string StartLatitudeErrorStatus = "Initialized With Invalid Latitude Value";
        static public string StartLongitudeErrorStatus = "Initialized With Invalid Longitude Value";
        static public string InvalidValueErrorStatus = "Got Invalid Value From Simulator";
        static public string TimeoutErrorStatus = "Timeout - The Simulator Is Not Responding";

        static public bool IsErrorStatus(string status)
        {
            return ((status == ConnectionFailedStatus) || (status == SimulatorDisconnectedStatus) || (status == TimeoutErrorStatus) 
                || (status == RcvErrorStatus)
                || (status == SendErrorStatus) || (status == LatitudeErrorStatus) || (status == LongitudeErrorStatus)
                || (status == StartLatitudeErrorStatus) || (status == StartLongitudeErrorStatus) || (status == InvalidValueErrorStatus));
        }
    }
}
