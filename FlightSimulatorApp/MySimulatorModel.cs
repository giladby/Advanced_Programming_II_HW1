using System;
using System.Threading;
using System.Collections.Generic;
using Microsoft.Maps.MapControl.WPF;

namespace FlightSimulatorApp
{
    class MySimulatorModel : ISimulatorModel
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private Queue<string> setMsgs = new Queue<string>();
        private ISimulatorClient client;
        private Object myLock;
        private volatile bool connected;
        private System.Timers.Timer myStatusTimer;
        private double oldLatitude;
        private double oldLongitude;
        private bool firstRotate;
        private bool firstValidLatitude;
        private bool firstValidLongitude;
        private bool firstPlaneAppearance;

        public MySimulatorModel(ISimulatorClient c)
        {
            client = c;
            // Make a lock object.
            myLock = new Object();
            connected = false;
            firstRotate = true;
            TurnOffLocationFlags();
            // Set the status timer to act every 5 seconds.
            myStatusTimer = new System.Timers.Timer { Interval = 5000, AutoReset = true, Enabled = true };
            myStatusTimer.Elapsed += StatusTimerOnTimedEvent;
        }

        string headingDeg;

        public string HeadingDeg
        {
            get
            {
                return headingDeg;
            }
            set
            {
                if (headingDeg != value)
                {
                    headingDeg = value;
                    NotifyPropertyChanged("HeadingDeg");
                }
            }
        }

        string verticalSpeed;

        public string VerticalSpeed
        {
            get
            {
                return verticalSpeed;
            }
            set
            {
                if (verticalSpeed != value)
                {
                    verticalSpeed = value;
                    NotifyPropertyChanged("VerticalSpeed");
                }
            }
        }

        string groundSpeedKt;

        public string GroundSpeedKt
        {
            get
            {
                return groundSpeedKt;
            }
            set
            {
                if (groundSpeedKt != value)
                {
                    groundSpeedKt = value;
                    NotifyPropertyChanged("GroundSpeedKt");
                }
            }
        }

        string indicatedSpeedKt;

        public string IndicatedSpeedKt
        {
            get
            {
                return indicatedSpeedKt;
            }
            set
            {
                if (indicatedSpeedKt != value)
                {
                    indicatedSpeedKt = value;
                    NotifyPropertyChanged("IndicatedSpeedKt");
                }
            }
        }

        string gpsIndicatedAltitudeFt;

        public string GpsIndicatedAltitudeFt
        {
            get
            {
                return gpsIndicatedAltitudeFt;
            }
            set
            {
                if (gpsIndicatedAltitudeFt != value)
                {
                    gpsIndicatedAltitudeFt = value;
                    NotifyPropertyChanged("GpsIndicatedAltitudeFt");
                }
            }
        }

        string rollDeg;

        public string RollDeg
        {
            get
            {
                return rollDeg;
            }
            set
            {
                if (rollDeg != value)
                {
                    rollDeg = value;
                    NotifyPropertyChanged("RollDeg");
                }
            }
        }

        string pitchDeg;

        public string PitchDeg
        {
            get
            {
                return pitchDeg;
            }
            set
            {
                if (pitchDeg != value)
                {
                    pitchDeg = value;
                    NotifyPropertyChanged("PitchDeg");
                }
            }
        }

        string altimeterIndicatedAltitudeFt;

        public string AltimeterIndicatedAltitudeFt
        {
            get
            {
                return altimeterIndicatedAltitudeFt;
            }
            set
            {
                if (altimeterIndicatedAltitudeFt != value)
                {
                    altimeterIndicatedAltitudeFt = value;
                    NotifyPropertyChanged("AltimeterIndicatedAltitudeFt");
                }
            }
        }

        string latitude;

        public string Latitude
        {
            get
            {
                return latitude;
            }
            set
            {
                double doubleValue;
                if (Double.TryParse(value, out doubleValue))
                {
                    // Valid latitude values are between -90 and 90.
                    if ((doubleValue > 90) || (doubleValue < -90))
                    {
                        // If already got the first valid latitude and longitude values.
                        if (firstValidLatitude && firstValidLongitude)
                        {
                            Status = MyStatus.latitudeErrorStatus;
                        }
                        else
                        {
                            Status = MyStatus.startLatitudeErrorStatus;
                        }
                    }
                    else
                    {
                        latitude = value;
                        NotifyPropertyChanged("Latitude");
                        firstValidLatitude = true;
                    }
                }
                else
                {
                    if (firstValidLatitude && firstValidLongitude)
                    {
                        Status = MyStatus.latitudeErrorStatus;
                    }
                    else
                    {
                        Status = MyStatus.startLatitudeErrorStatus;
                    }
                    latitude = value;
                    NotifyPropertyChanged("Latitude");
                }
            }
        }

        string longitude;

        public string Longitude
        {
            get
            {
                return longitude;
            }
            set
            {
                double doubleValue;
                if (Double.TryParse(value, out doubleValue))
                {
                    // Valid longitude values are between -180 and 180.
                    if ((doubleValue > 180) || (doubleValue < -180))
                    {
                        // If already got the first valid latitude and longitude values.
                        if (firstValidLatitude && firstValidLongitude)
                        {
                            Status = MyStatus.longitudeErrorStatus;
                        }
                        else
                        {
                            Status = MyStatus.startLongitudeErrorStatus;
                        }
                    }
                    else
                    {
                        longitude = value;
                        NotifyPropertyChanged("Longitude");
                        firstValidLongitude = true;
                    }
                }
                else
                {
                    if (firstValidLatitude && firstValidLongitude)
                    {
                        Status = MyStatus.longitudeErrorStatus;
                    }
                    else
                    {
                        Status = MyStatus.startLongitudeErrorStatus;
                    }
                    longitude = value;
                    NotifyPropertyChanged("Longitude");
                }
            }
        }

        double angle;

        public double Angle
        {
            get
            {
                return angle;
            }
            set
            {
                if (angle != value)
                {
                    angle = value;
                    NotifyPropertyChanged("Angle");
                }
            }
        }

        Location planeLocation;

        public Location PlaneLocation
        {
            get
            {
                return planeLocation;
            }
            set
            {
                // If this is a new location.
                if ((planeLocation == null) || (planeLocation.Latitude != value.Latitude) || (planeLocation.Longitude != value.Longitude))
                {
                    planeLocation = value;
                    NotifyPropertyChanged("PlaneLocation");
                }
            }
        }

        string status;

        public string Status
        {
            get
            {
                return status;
            }
            set
            {
                // If this is a status of no connection.
                if ((value == MyStatus.disconnectedStatus) || (value == MyStatus.simulatorDisconnectedStatus) || (value == MyStatus.notConnectedStatus))
                {
                    connected = false;
                    firstRotate = true;
                    ClearAllParameters();
                }
                // If this is not a "Connected" or "Not Connected" status when the current status is already one of those.
                if (!((value == status) && ((value == MyStatus.notConnectedStatus) || (value == MyStatus.connectedStatus))))
                {
                    status = value;
                    NotifyPropertyChanged("Status");
                    ResetStatusTimer();
                }
            }
        }

        // Make a message to add to the messages queue.
        public void AddSetString(string name, string value)
        {
            string msg = $"set {name} {value}\n";
            lock (myLock)
            {
                setMsgs.Enqueue(msg);
            }
        }

        public void Connect(string ip, int port)
        {
            TurnOffLocationFlags();
            Status = MyStatus.tryingToConnectStatus;
            // Trying to connect to the given IP and port on a thread so the application won't stuck.
            new Thread(delegate ()
            {
                string result = client.Connect(ip, port);
                if (result == MyStatus.connectedStatus)
                {
                    connected = true;
                    Start();
                }
                Status = result;
            }).Start();
        }

        public void Disconnect()
        {
            connected = false;
            client.Disconnect();
            Status = MyStatus.disconnectedStatus;
        }

        private void RecvData(string property)
        {
            double value;
            string rcvStatus = client.Receive();
            // If this is not an error or a disconnection.
            if ((rcvStatus != MyStatus.rcvErrorStatus) && (rcvStatus != MyStatus.simulatorDisconnectedStatus))
            {
                if (rcvStatus == "ERR\n")
                {
                    Status = MyStatus.rcvErrorStatus;
                    switchValues(property, "ERR");
                    return;
                }
                // If the value is not a number.
                if (!Double.TryParse(rcvStatus, out value))
                {
                    // Latitude and longitude has their own error status.
                    if ((property != "Latitude") && (property != "Longitude"))
                    {
                        Status = MyStatus.invalidValueErrorStatus;
                    }
                    switchValues(property, "ERR");
                    return;
                }
                value = Math.Round(value, 6);
                // Set the right property with the received value.
                switchValues(property, value.ToString());
            }
            else
            {
                Status = rcvStatus;
            }
        }

        private void switchValues(string property, string value)
        {
            switch (property)
            {
                case "HeadingDeg":
                    HeadingDeg = value;
                    break;
                case "VerticalSpeed":
                    VerticalSpeed = value;
                    break;
                case "GroundSpeedKt":
                    GroundSpeedKt = value;
                    break;
                case "IndicatedSpeedKt":
                    IndicatedSpeedKt = value;
                    break;
                case "GpsIndicatedAltitudeFt":
                    GpsIndicatedAltitudeFt = value;
                    break;
                case "RollDeg":
                    RollDeg = value;
                    break;
                case "PitchDeg":
                    PitchDeg = value;
                    break;
                case "AltimeterIndicatedAltitudeFt":
                    AltimeterIndicatedAltitudeFt = value;
                    break;
                case "Latitude":
                    Latitude = value;
                    break;
                case "Longitude":
                    Longitude = value;
                    break;
            }
        }

        public void Start()
        {
            string msg;
            double doubleLat;
            double doubleLong;
            bool validLat = false;
            bool validLong = false;
            // Send messages to the simulator on a thread so the application won't stuck.
            new Thread(delegate ()
            {
                while (connected)
                {
                    lock (myLock)
                    {
                        // While there are still messages to send to the simulator.
                        while (setMsgs.Count != 0)
                        {
                            msg = setMsgs.Dequeue();
                            string sendStatus = client.Send(msg);
                            if (sendStatus != MyStatus.okStatus)
                            {
                                if (sendStatus == "ERR\n")
                                {
                                    Status = MyStatus.sendErrorStatus;
                                }
                                else
                                {
                                    Status = sendStatus;
                                }
                            }
                            else
                            {
                                // Receive the coming back message from the simulator.
                                string rcvStatus = client.Receive();
                                if ((rcvStatus != MyStatus.rcvErrorStatus) && (rcvStatus != MyStatus.simulatorDisconnectedStatus))
                                {
                                    if (rcvStatus == "ERR\n")
                                    {
                                        Status = MyStatus.sendErrorStatus;
                                    }
                                }
                                else
                                {
                                    Status = rcvStatus;
                                }
                            }
                        }
                    }
                    // Send all the parameters getters to the simulator and receive the response.
                    client.Send("get /instrumentation/heading-indicator/indicated-heading-deg\n");
                    RecvData("HeadingDeg");
                    client.Send("get /instrumentation/gps/indicated-vertical-speed\n");
                    RecvData("VerticalSpeed");
                    client.Send("get /instrumentation/gps/indicated-ground-speed-kt\n");
                    RecvData("GroundSpeedKt");
                    client.Send("get /instrumentation/airspeed-indicator/indicated-speed-kt\n");
                    RecvData("IndicatedSpeedKt");
                    client.Send("get /instrumentation/gps/indicated-altitude-ft\n");
                    RecvData("GpsIndicatedAltitudeFt");
                    client.Send("get /instrumentation/attitude-indicator/internal-roll-deg\n");
                    RecvData("RollDeg");
                    client.Send("get /instrumentation/attitude-indicator/internal-pitch-deg\n");
                    RecvData("PitchDeg");
                    client.Send("get /instrumentation/altimeter/indicated-altitude-ft\n");
                    RecvData("AltimeterIndicatedAltitudeFt");
                    client.Send("get /position/latitude-deg\n");
                    RecvData("Latitude");
                    client.Send("get /position/longitude-deg\n");
                    RecvData("Longitude");
                    // If this is before the airplane has appeared.
                    if (!firstPlaneAppearance)
                    {
                        if (firstValidLatitude && firstValidLongitude)
                        {
                            if (connected)
                            {
                                Status = MyStatus.connectedStatus;
                            }
                            firstPlaneAppearance = true;
                        }
                    }
                    // Rotate the airplane and set a new location for it.
                    RotateAirplane();
                    // If the latitude and longitude are valid.
                    if (Double.TryParse(Latitude, out doubleLat))
                    {
                        oldLatitude = doubleLat;
                        validLat = true;
                    } else
                    {
                        validLat = false;
                    }
                    if (Double.TryParse(Longitude, out doubleLong))
                    {
                        oldLongitude = doubleLong;
                        validLong = true;
                    } else
                    {
                        validLong = false;
                    }
                    // If the new location is valid.
                    if (firstPlaneAppearance && validLat && validLong)
                    {
                        PlaneLocation = new Location(doubleLat, doubleLong);
                    }
                    // Does this method 4 times in a second.
                    Thread.Sleep(250);
                }
            }).Start();
        }

        // Rotates the aiplane angle according to the new location he goes to.
        private void RotateAirplane()
        {
            double doubleLat;
            double doubleLong;
            if (!(Double.TryParse(Latitude, out doubleLat) && Double.TryParse(Longitude, out doubleLong)))
            {
                if (firstRotate)
                {
                    Angle = 0;
                }
                return;
            }
            if (firstRotate)
            {
                Angle = 0;
                firstRotate = false;
                return;
            }
            double y = doubleLat - oldLatitude;
            double x = doubleLong - oldLongitude;
            double angle;
            // If the airplane goes up.
            if (x == 0)
            {
                if (y == 0)
                {
                    return;
                }
                if (y < 0)
                {
                    angle = 270;
                }
                else
                {
                    angle = 90;
                }
            }
            else
            {
                // Calculates the angle and set it to degrees.
                angle = Math.Atan(y / x) * (180 / Math.PI);
                if (x < 0)
                {
                    angle += 180;
                }
            }
            Angle = angle * -1;
        }

        public void NotifyPropertyChanged(string propName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propName));
            }
        }

        // Set the status to "Connected" or "Not Connected" with the timer.
        public void StatusTimerOnTimedEvent(Object source, System.Timers.ElapsedEventArgs e)
        {
            if (connected)
            {
                Status = MyStatus.connectedStatus;
            }
            else
            {
                Status = MyStatus.notConnectedStatus;
            }
        }

        private void ResetStatusTimer()
        {
            myStatusTimer.Stop();
            myStatusTimer.Start();
        }

        private void TurnOffLocationFlags()
        {
            firstValidLatitude = false;
            firstValidLongitude = false;
            firstPlaneAppearance = false;
        }

        private void ClearAllParameters()
        {
            HeadingDeg = "";
            VerticalSpeed = "";
            GroundSpeedKt = "";
            IndicatedSpeedKt = "";
            GpsIndicatedAltitudeFt = "";
            RollDeg = "";
            PitchDeg = "";
            AltimeterIndicatedAltitudeFt = "";
            Latitude = "";
            Longitude = "";
        }
    }
}
