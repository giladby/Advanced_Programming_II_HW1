using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlightSimulatorApp
{
    class SimulatorViewModel
    {
        private ISimulatorModel model;
        public SimulatorViewModel(ISimulatorModel m)
        {
            this.model = m;
            model.PropertyChanged += delegate (Object sender, PropertyChangedEventArgs e)
            {
                setProperty("VM_" + e.getPropName());
            };
            
        }

        public void connect(string ip, int port)
        {
            VM_Status = "hello";

            //model.connect(ip, port);

        }

        double VM_headingDeg;
        public double VM_HeadingDeg
        {
            get
            {
                return VM_headingDeg;
            }
            set
            {
                if (VM_headingDeg != value)
                {
                    VM_headingDeg = value;
                }
            }
        }
        double VM_verticalSpeed;
        public double VM_VerticalSpeed
        {
            get
            {
                return VM_verticalSpeed;
            }
            set
            {
                if (VM_verticalSpeed != value)
                {
                    VM_verticalSpeed = value;
                }
            }
        }
        double VM_groundSpeedKt;
        public double VM_GroundSpeedKt
        {
            get
            {
                return VM_groundSpeedKt;
            }
            set
            {
                if (VM_groundSpeedKt != value)
                {
                    VM_groundSpeedKt = value;
                }
            }
        }
        double VM_indicatedSpeedKt;
        public double VM_IndicatedSpeedKt
        {
            get
            {
                return VM_indicatedSpeedKt;
            }
            set
            {
                if (VM_indicatedSpeedKt != value)
                {
                    VM_indicatedSpeedKt = value;
                }
            }
        }
        double VM_gpsIndicatedAltitudeFt;
        public double VM_GpsIndicatedAltitudeFt
        {
            get
            {
                return VM_gpsIndicatedAltitudeFt;
            }
            set
            {
                if (VM_gpsIndicatedAltitudeFt != value)
                {
                    VM_gpsIndicatedAltitudeFt = value;
                }
            }
        }
        double VM_rollDeg;
        public double VM_RollDeg
        {
            get
            {
                return VM_rollDeg;
            }
            set
            {
                if (VM_rollDeg != value)
                {
                    VM_rollDeg = value;
                }
            }
        }
        double VM_pitchDeg;
        public double VM_PitchDeg
        {
            get
            {
                return VM_pitchDeg;
            }
            set
            {
                if (VM_pitchDeg != value)
                {
                    VM_pitchDeg = value;
                }
            }
        }
        double VM_altimeterIndicatedAltitudeFt;
        public double VM_AltimeterIndicatedAltitudeFt
        {
            get
            {
                return VM_altimeterIndicatedAltitudeFt;
            }
            set
            {
                if (VM_altimeterIndicatedAltitudeFt != value)
                {
                    VM_altimeterIndicatedAltitudeFt = value;
                }
            }
        }

        string VM_status;
        public string VM_Status
        {
            get
            {
                return VM_status;
            }
            set
            {
                Console.WriteLine("error in vm status prop");
                VM_status = value;
            }
        }

        private void setProperty(string propName)
        {
            switch(propName)
            {
                case "VM_HeadingDeg":
                    VM_HeadingDeg = model.HeadingDeg;
                    break;
                case "VM_verticalSpeed":
                    VM_VerticalSpeed = model.VerticalSpeed;
                    break;
                case "VM_GroundSpeedKt":
                    VM_GroundSpeedKt = model.GroundSpeedKt;
                    break;
                case "VM_IndicatedSpeedKt":
                    VM_IndicatedSpeedKt = model.IndicatedSpeedKt;
                    break;
                case "VM_GpsIndicatedAltitudeFt":
                    VM_GpsIndicatedAltitudeFt = model.GpsIndicatedAltitudeFt;
                    break;
                case "VM_RollDeg":
                    VM_RollDeg = model.RollDeg;
                    break;
                case "VM_PitchDeg":
                    VM_PitchDeg = model.PitchDeg;
                    break;
                case "VM_AltimeterIndicatedAltitudeFt":
                    VM_AltimeterIndicatedAltitudeFt = model.AltimeterIndicatedAltitudeFt;
                    break;
                case "VM_Status":
                    VM_Status = model.Status;
                    break;
            }
        }
        double VM_elevator;
        public double VM_Elevator
        {
            get
            {
                return VM_headingDeg;
            }
            set
            {
                if (VM_elevator != value)
                {
                    VM_elevator = value;
                    Console.WriteLine($"change elevator to : {value}");
                    model.addSetString("/controls/flight/elevator", VM_elevator);
                }
            }
        }
        double VM_rudder;
        public double VM_Rudder
        {
            get
            {
                return VM_rudder;
            }
            set
            {
                if (VM_rudder != value)
                {
                    VM_rudder = value;
                    Console.WriteLine($"change rudder to : {value}");
                    model.addSetString("/controls/flight/rudder", VM_rudder);
                }
            }
        }
        double VM_aileron;
        public double VM_Aileron
        {
            get
            {
                return VM_aileron;
            }
            set
            {
                if (VM_aileron != value)
                {
                    VM_aileron = value;
                    model.addSetString("/controls/flight/aileron", VM_aileron);
                }
            }
        }
        double VM_throttle;
        public double VM_Throttle
        {
            get
            {
                return VM_throttle;
            }
            set
            {
                if (VM_throttle != value)
                {
                    VM_throttle = value;
                    model.addSetString("/controls/engines/engine/throttle", VM_throttle);
                }
            }
        }
    }
}
