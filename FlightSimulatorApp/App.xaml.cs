using System.Windows;

namespace FlightSimulatorApp
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public SimulatorControlsViewModel ControlsVM { get; internal set; }
        public SimulatorDashboardViewModel DashboardVM { get; internal set; }
        public SimulatorMapViewModel MapVM { get; internal set; }
        
        private void Application_Startup(object sender, StartupEventArgs e)
        {
            ISimulatorClient client = new MySimulatorClient();
            ISimulatorModel model = new MySimulatorModel(client);
            ControlsVM = new SimulatorControlsViewModel(model);
            DashboardVM = new SimulatorDashboardViewModel(model);
            MapVM = new SimulatorMapViewModel(model);
        }
    }
}
