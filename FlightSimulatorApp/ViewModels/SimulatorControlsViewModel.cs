using System.Windows.Controls;

namespace FlightSimulatorApp
{
    public class SimulatorControlsViewModel : UserControl
    {
        private ISimulatorModel model;
       
        public SimulatorControlsViewModel(ISimulatorModel m)
        {
            model = m;
        }

        private string vmElevator;

        public string VmElevator
        {
            get
            {
                return vmElevator;
            }
            set
            {
                if (vmElevator != value)
                {
                    vmElevator = value;
                    model.AddSetString("/controls/flight/elevator", vmElevator);
                }
            }
        }

        private string vmRudder;

        public string VmRudder
        {
            get
            {
                return vmRudder;
            }
            set
            {
                if (vmRudder != value)
                {
                    vmRudder = value;
                    model.AddSetString("/controls/flight/rudder", vmRudder);
                }
            }
        }

        private string vmAileron;

        public string VmAileron
        {
            get
            {
                return vmAileron;
            }
            set
            {
                if (vmAileron != value)
                {
                    vmAileron = value;
                    model.AddSetString("/controls/flight/aileron", vmAileron);
                }
            }
        }

        private string vmThrottle;

        public string VmThrottle
        {
            get
            {
                return vmThrottle;
            }
            set
            {
                if (vmThrottle != value)
                {
                    vmThrottle = value;
                    model.AddSetString("/controls/engines/current-engine/throttle", vmThrottle);
                }
            }
        }

        public void Connect(string ip, string port)
        {
            model.Connect(ip, port);
        }

        public void Disconnect()
        {
            model.Disconnect();
        }
    }
}
