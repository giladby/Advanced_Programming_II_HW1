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
        volatile bool connected;
        System.Timers.Timer myStatusTimer;
        int statusTimerTime;

        public MySimulatorModel(ISimulatorClient c)
        {
            this.client = c;
            myLock = new Object();
            connected = false;
            myStatusTimer = new System.Timers.Timer();
            statusTimerTime = 10000;
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

        string status;
        public string Status
        {
            get
            {
                return status;
            }
            set
            {
                if (value == MainWindow.disconnectedStatus)
                {
                    connected = false;
                }
                status = value;
                NotifyPropertyChanged("Status");
                resetStatusTimer();
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
            
            Console.WriteLine("msg in addSetString: " + setMsgs.Count.ToString());
        }

        public void connect(string ip, int port)
        {
            string result = client.connect(ip, port);
            if(result == MainWindow.connectedStatus)
            {
                connected = true;
                start();
            }
            Status = result;
        }


        private void recvData(string property)
        {
            double value;
            string rcvStatus = client.recieve();
            Console.WriteLine(rcvStatus + "in function");
            if (rcvStatus != MainWindow.rcvErrorStatus && rcvStatus != MainWindow.disconnectedStatus)
            {
                if(rcvStatus == "ERR\n")
                {
                    Status = MainWindow.rcvErrorStatus;
                    return;
                }
                
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
            // set the status timer
            myStatusTimer.Interval = statusTimerTime;
            myStatusTimer.Elapsed += StatusTimerOnTimedEvent;
            myStatusTimer.AutoReset = true;
            myStatusTimer.Enabled = true;

            string msg;

            // set and get thread
            new Thread(delegate ()
            {
                while (connected)
                {
                    Console.WriteLine("before locking");
                    lock (myLock)
                    {
                        Console.WriteLine("msg in start: " + setMsgs.Count.ToString());
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
                        }
                    }
                    Console.WriteLine("after locking");
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
            if (this.PropertyChanged != null)
            {
                this.PropertyChanged(this, new PropertyChangedEventArgs(propName));
            }
        }

        public void StatusTimerOnTimedEvent(Object source, System.Timers.ElapsedEventArgs e)
        {
            if (connected)
            {
                Status = MainWindow.connectedStatus;
            }
        }

        private void resetStatusTimer()
        {
            myStatusTimer.Stop();
            myStatusTimer.Start();
        }
    }
}
