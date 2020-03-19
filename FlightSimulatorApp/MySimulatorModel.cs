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
            if(result == "Connected to simulator.")
            {
                start();
            }
            Status = result;
        }

        private void recvData(string property)
        {
            double value;
            string rcv = client.recieve();
            if (rcv != "Error trying to recieve data from simulator." && rcv != "Disconnected from simulator.")
            {
                value = Double.Parse(rcv);
                switch (property)
                {
                    case "":
                        break;
                }
            }
            else
            {
                Status = rcv;
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
                            client.send(setMsgs.Dequeue());
                        }
                    }

                    client.send("get /instrumentation/heading-indicator/indicated-heading-deg");
                    recvData("HeadingDeg");
                    client.send("get /instrumentation/gps/indicated-vertical-speed");
                    recvData("VerticalSpeed");
                    client.send("get /instrumentation/gps/indicated-ground-speed-kt");
                    GroundSpeedKt = Double.Parse(client.recieve());
                    client.send("get /instrumentation/airspeed-indicator/indicated-speed-kt");
                    IndicatedSpeedKt = Double.Parse(client.recieve());
                    client.send("get /instrumentation/encoder/indicated-altitude-ft");
                    GpsIndicatedAltitudeFt = Double.Parse(client.recieve());
                    client.send("get /instrumentation/attitude-indicator/internal-roll-deg");
                    RollDeg = Double.Parse(client.recieve());
                    client.send("get /instrumentation/attitude-indicator/internal-pitch-deg");
                    PitchDeg = Double.Parse(client.recieve());
                    client.send("get /instrumentation/altimeter/indicated-altitude-ft");
                    AltimeterIndicatedAltitudeFt = Double.Parse(client.recieve());

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
