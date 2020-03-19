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
        static public string connectedStatus = "Connected to simulator.";
        static public string connectionFailedStatus = "Failed connecting to simulator.";
        static public string disconnectedStatus = "Disconnected from simulator.";
        static public string rcvErrorStatus = "Error trying to recieve data from simulator.";
        static public string sendErrorStatus = "Error trying to send data from simulator.";
        static public string okStatus = "OK";
        Queue<string> setMsgs = new Queue<string>();
        ISimulatorClient client;
        volatile bool stop;
        private Object myLock;

        public MySimulatorModel(ISimulatorClient c)
        {
            this.client = c;
            stop = false;
            myLock = new Object();
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

        string stauts;
        public string Status
        {
            get
            {
                return stauts;
            }
            set
            {
                stauts = value;
                NotifyPropertyChanged("Status");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public void addSetString(string name, double value)
        {
            string msg = "set " + name + " " + value.ToString();
            lock(myLock)
            {
                setMsgs.Enqueue(msg);
            }
        }

        public void connect(string ip, int port)
        {
            string result = client.connect(ip, port);
            if(result == MySimulatorModel.connectedStatus)
            {
                start();
            }
            Status = result;
        }

        private void recvData(string property)
        {
            double value;
            string rcvStatus = client.recieve();
            if (rcvStatus != MySimulatorModel.rcvErrorStatus && rcvStatus != MySimulatorModel.disconnectedStatus)
            {
                value = Double.Parse(rcvStatus);
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
                }
            }
            else
            {
                Status = rcvStatus;
            }
        }

        public void start()
        {
            new Thread(delegate ()
            {
                while (!stop)
                {
                    lock (myLock)
                    {
                        while (setMsgs.Count != 0)
                        {
                            string sendStatus = client.send(setMsgs.Dequeue());
                            if (sendStatus != MySimulatorModel.okStatus)
                            {
                                Status = sendStatus;
                            }
                        }
                    }

                    client.send("get /instrumentation/heading-indicator/indicated-heading-deg");
                    recvData("HeadingDeg");
                    client.send("get /instrumentation/gps/indicated-vertical-speed");
                    recvData("VerticalSpeed");
                    client.send("get /instrumentation/gps/indicated-ground-speed-kt");
                    recvData("GroundSpeedKt");
                    client.send("get /instrumentation/airspeed-indicator/indicated-speed-kt");
                    recvData("IndicatedSpeedKt");
                    client.send("get /instrumentation/encoder/indicated-altitude-ft");
                    recvData("GpsIndicatedAltitudeFt");
                    client.send("get /instrumentation/attitude-indicator/internal-roll-deg");
                    recvData("RollDeg");
                    client.send("get /instrumentation/attitude-indicator/internal-pitch-deg");
                    recvData("PitchDeg");
                    client.send("get /instrumentation/altimeter/indicated-altitude-ft");
                    recvData("AltimeterIndicatedAltitudeFt");

                    Thread.Sleep(250);
                }
            }).Start();
        }

        public void NotifyPropertyChanged(string propName)
        {
            if(this.PropertyChanged != null)
            {
                this.PropertyChanged(this, new PropertyChangedEventArgs(propName));
            }
        }
    }
}
