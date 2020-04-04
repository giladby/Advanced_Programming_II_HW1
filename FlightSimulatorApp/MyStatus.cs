
namespace FlightSimulatorApp
{
    class MyStatus
    {
        // All the possible statuses of this program.
        static public string okStatus = "OK";
        static public string connectedStatus = "Connected";
        static public string notConnectedStatus = "Not Connected";
        static public string tryingToConnectStatus = "Trying To Connect...";
        static public string disconnectedStatus = "Disconnected From Simulator";
        static public string connectionFailedStatus = "Failed Connecting To Simulator";
        static public string simulatorDisconnectedStatus = "The Simulator Disconnected";
        static public string rcvErrorStatus = "Error Trying To Receive Data From Simulator";
        static public string sendErrorStatus = "Error Trying To Send Data To Simulator";
        static public string latitudeErrorStatus = "Received Invalid Latitude Value";
        static public string longitudeErrorStatus = "Received Invalid Longitude Value";
        static public string startLatitudeErrorStatus = "Initialized With Invalid Latitude Value";
        static public string startLongitudeErrorStatus = "Initialized With Invalid Longitude Value";

        static public bool IsErrorStatus(string status)
        {
            return ((status == connectionFailedStatus) || (status == simulatorDisconnectedStatus) || (status == rcvErrorStatus)
                || (status == sendErrorStatus) || (status == latitudeErrorStatus) || (status == longitudeErrorStatus)
                || (status == startLatitudeErrorStatus) || (status == startLongitudeErrorStatus));
        }
    }
}
