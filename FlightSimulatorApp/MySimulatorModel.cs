using System;
using System.Threading;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
            firstValidLatitude = false;
            firstValidLongitude = false;
            firstPlaneAppearance = false;

            // set the status timer 
            myStatusTimer = new System.Timers.Timer();
            myStatusTimer.Interval = 5000;
            myStatusTimer.Elapsed += StatusTimerOnTimedEvent;
            myStatusTimer.AutoReset = true;
            myStatusTimer.Enabled = true;
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
                if(headingDeg != value)
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
                    if (value > 90 || value < -90)
                    {
                        if (firstValidLatitude && firstValidLongitude)
                        {
                            Status = MyStatus.latitudeErrorStatus;
                        } else
                        {
                            Status = MyStatus.startLatitudeErrorStatus;
                        }
                    } else
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
                    if (value > 180 || value < -180)
                    {
                        if (firstValidLatitude && firstValidLongitude)
                        {
                            Status = MyStatus.longitudeErrorStatus;
                        }
                        else
                        {
                            Status = MyStatus.startLongitudeErrorStatus;

                        }
                    } else
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
                if (planeLocation == null || planeLocation.Latitude != value.Latitude || planeLocation.Longitude != value.Longitude)
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
                if (value == MyStatus.disconnectedStatus || value == MyStatus.notConnectedStatus)
                {
                    connected = false;
                    firstRotate = true;
                }
                if (!(value == status && (value == MyStatus.notConnectedStatus || value == MyStatus.connectedStatus)))
                {
                    status = value;
                    NotifyPropertyChanged("Status");
                    resetStatusTimer();
                }
            }
        }

        public void addSetString(string name, double value)
        {
            string msg = "set " + name + " " + value.ToString() + "\n";

            lock (myLock)
            {
                setMsgs.Enqueue(msg);
            }
        }

        public void connect(string ip, int port)
        {
            Status = MyStatus.tryingToConnectStatus;
            new Thread(delegate ()
            {
                string result = client.connect(ip, port);
                if (result == MyStatus.connectedStatus)
                {
                    connected = true;
                    start();
                }
                Status = result;
            }).Start();
        }


        private void recvData(string property)
        {
            double value;
            string rcvStatus = client.recieve();
            if (rcvStatus != MyStatus.rcvErrorStatus && rcvStatus != MyStatus.disconnectedStatus)
            {
                if(rcvStatus == "ERR\n")
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

        public void start()
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
                            Console.WriteLine(msg);
                            string sendStatus = client.send(msg);
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
                                string rcvStatus = client.recieve();
                                if (rcvStatus != MyStatus.rcvErrorStatus && rcvStatus != MyStatus.disconnectedStatus)
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
                    client.send("get /instrumentation/heading-indicator/indicated-heading-deg\n");
                    recvData("HeadingDeg");
                    client.send("get /instrumentation/gps/indicated-vertical-speed\n");
                    recvData("VerticalSpeed");
                    client.send("get /instrumentation/gps/indicated-ground-speed-kt\n");
                    recvData("GroundSpeedKt");
                    client.send("get /instrumentation/airspeed-indicator/indicated-speed-kt\n");
                    recvData("IndicatedSpeedKt");
                    client.send("get /instrumentation/gps/indicated-altitude-ft\n");
                    recvData("GpsIndicatedAltitudeFt");
                    client.send("get /instrumentation/attitude-indicator/internal-roll-deg\n");
                    recvData("RollDeg");
                    client.send("get /instrumentation/attitude-indicator/internal-pitch-deg\n");
                    recvData("PitchDeg");
                    client.send("get /instrumentation/altimeter/indicated-altitude-ft\n");
                    recvData("AltimeterIndicatedAltitudeFt");

                    client.send("get /position/latitude-deg\n");
                    recvData("Latitude");
                    client.send("get /position/longitude-deg\n");
                    recvData("Longitude");

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

                    rotateAirplane();
                    oldLatitude = Latitude;
                    oldLongitude = Longitude;

                    PlaneLocation = new Location(Latitude, Longitude);

                    Thread.Sleep(250);
                }
            }).Start();
        }

        private void rotateAirplane()
        {
            if(firstRotate)
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

        private void resetStatusTimer()
        {
            myStatusTimer.Stop();
            myStatusTimer.Start();
        }
    }
}
