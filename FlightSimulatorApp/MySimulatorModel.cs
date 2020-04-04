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
            myLock = new Object();
            connected = false;
            firstRotate = true;
            TurnOffLocationFlags();
            // set the status timer 
            myStatusTimer = new System.Timers.Timer { Interval = 5000, AutoReset = true, Enabled = true };
            myStatusTimer.Elapsed += StatusTimerOnTimedEvent;
        }

        double headingDeg;

        public double HeadingDeg
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

        double verticalSpeed;

        public double VerticalSpeed
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

        double groundSpeedKt;

        public double GroundSpeedKt
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

        double indicatedSpeedKt;

        public double IndicatedSpeedKt
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

        double gpsIndicatedAltitudeFt;

        public double GpsIndicatedAltitudeFt
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

        double rollDeg;

        public double RollDeg
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

        double pitchDeg;

        public double PitchDeg
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

        double altimeterIndicatedAltitudeFt;

        public double AltimeterIndicatedAltitudeFt
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

        double latitude;

        public double Latitude
        {
            get
            {
                return latitude;
            }
            set
            {
                if (latitude != value)
                {
                    if ((value > 90) || (value < -90))
                    {
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
            }
        }

        double longitude;

        public double Longitude
        {
            get
            {
                return longitude;
            }
            set
            {
                if (longitude != value)
                {
                    if ((value > 180) || (value < -180))
                    {
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
                if ((value == MyStatus.disconnectedStatus) || (value == MyStatus.simulatorDisconnectedStatus) || (value == MyStatus.notConnectedStatus))
                {
                    connected = false;
                    firstRotate = true;
                    ClearAllParameters();
                }
                if (!((value == status) && ((value == MyStatus.notConnectedStatus) || (value == MyStatus.connectedStatus))))
                {
                    status = value;
                    NotifyPropertyChanged("Status");
                    ResetStatusTimer();
                }
            }
        }

        public void AddSetString(string name, double value)
        {
            string msg = $"set {name} {value.ToString()}\n";
            lock (myLock)
            {
                setMsgs.Enqueue(msg);
            }
        }

        public void Connect(string ip, int port)
        {
            TurnOffLocationFlags();
            Status = MyStatus.tryingToConnectStatus;
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
            string rcvStatus = client.Recieve();
            if ((rcvStatus != MyStatus.rcvErrorStatus) && (rcvStatus != MyStatus.simulatorDisconnectedStatus))
            {
                if (rcvStatus == "ERR\n")
                {
                    Status = MyStatus.rcvErrorStatus;
                    return;
                }
                value = Math.Round(Double.Parse(rcvStatus), 6);
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
            else
            {
                Status = rcvStatus;
            }
        }

        public void Start()
        {
            string msg;
            // set and get thread
            new Thread(delegate ()
            {
                while (connected)
                {
                    lock (myLock)
                    {
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
                                string rcvStatus = client.Recieve();
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
                    RotateAirplane();
                    oldLatitude = Latitude;
                    oldLongitude = Longitude;
                    PlaneLocation = new Location(Latitude, Longitude);
                    Thread.Sleep(250);
                }
            }).Start();
        }

        private void RotateAirplane()
        {
            if (firstRotate)
            {
                Angle = 0;
                firstRotate = false;
                return;
            }
            double y = Latitude - oldLatitude;
            double x = Longitude - oldLongitude;
            double angle;
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
            HeadingDeg = 0;
            VerticalSpeed = 0;
            GroundSpeedKt = 0;
            IndicatedSpeedKt = 0;
            GpsIndicatedAltitudeFt = 0;
            RollDeg = 0;
            PitchDeg = 0;
            AltimeterIndicatedAltitudeFt = 0;
            Latitude = 0;
            Longitude = 0;
        }
    }
}
