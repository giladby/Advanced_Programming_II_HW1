using System;
using System.Threading;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlightSimulatorApp
{
    class MySimulatorModel : ISimulatorModel
    {
        Queue<string> setMsgs = new Queue<string>();
        ISimulatorClient client;
        private Object myLock;
        private Object statusLock;
        volatile bool connected;
        System.Timers.Timer myStatusTimer;

        public MySimulatorModel(ISimulatorClient c)
        {
            this.client = c;
            myLock = new Object();
            statusLock = new Object();
            connected = false;

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
                    latitude = value;
                    NotifyPropertyChanged("Latitude");
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
                    longitude = value;
                    NotifyPropertyChanged("Longitude");
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
                //lock(statusLock)
                //{
                //    if (value == MainWindow.disconnectedStatus || value == MainWindow.notConnectedStatus)
                //    {
                //        connected = false;
                //        resetAllParameters();
                //    }
                //    if (!(value == status && (value == MainWindow.notConnectedStatus || value == MainWindow.connectedStatus)))
                //    {
                //        status = value;
                //        NotifyPropertyChanged("Status");
                //        resetStatusTimer();
                //    }
                //}
                if (value == MainWindow.disconnectedStatus || value == MainWindow.notConnectedStatus)
                {
                    connected = false;
                    resetAllParameters();
                }
                if (!(value == status && (value == MainWindow.notConnectedStatus || value == MainWindow.connectedStatus)))
                {
                    status = value;
                    NotifyPropertyChanged("Status");
                    resetStatusTimer();
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

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
            Status = MainWindow.tryingToConnectStatus;
            new Thread(delegate ()
            {
                string result = client.connect(ip, port);
                if (result == MainWindow.connectedStatus)
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
            if (rcvStatus != MainWindow.rcvErrorStatus && rcvStatus != MainWindow.disconnectedStatus)
            {
                if(rcvStatus == "ERR\n")
                {
                    Status = MainWindow.rcvErrorStatus;
                    return;
                }

                value = Math.Round(Double.Parse(rcvStatus), 5);


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
                            if (sendStatus != MainWindow.okStatus)
                            {
                                if (sendStatus == "ERR\n")
                                {
                                    Status = MainWindow.sendErrorStatus;
                                }
                                else
                                {
                                    Status = sendStatus;
                                }
                            }
                            else
                            {
                                string rcvStatus = client.recieve();
                                if (rcvStatus != MainWindow.rcvErrorStatus && rcvStatus != MainWindow.disconnectedStatus)
                                {
                                    if (rcvStatus == "ERR\n")
                                    {
                                        Status = MainWindow.sendErrorStatus;
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

                    Thread.Sleep(250);
                }
            }).Start();
        }

        public void NotifyPropertyChanged(string propName)
        {
            if (this.PropertyChanged != null)
            {
                this.PropertyChanged(this, new PropertyChangedEventArgs(propName));
            }
        }

        public void StatusTimerOnTimedEvent(Object source, System.Timers.ElapsedEventArgs e)
        {
            //lock(statusLock)
            //{
            //    if (connected)
            //    {
            //        Status = MainWindow.connectedStatus;
            //    }
            //    else
            //    {
            //        Status = MainWindow.notConnectedStatus;
            //    }
            //}
            if (connected)
            {
                Status = MainWindow.connectedStatus;
            }
            else
            {
                Status = MainWindow.notConnectedStatus;
            }
        }

        private void resetStatusTimer()
        {
            myStatusTimer.Stop();
            myStatusTimer.Start();
        }
        private void resetAllParameters()
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
