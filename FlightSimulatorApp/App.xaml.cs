using System.Windows;

namespace FlightSimulatorApp
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public SimulatorControlsViewModel controlsVM { get; internal set; }
        public SimulatorDashboardViewModel dashboardVM { get; internal set; }
        public SimulatorMapViewModel mapVM { get; internal set; }
        
        private void Application_Startup(object sender, StartupEventArgs e)
        {
            ISimulatorClient client = new MySimulatorClient();
            ISimulatorModel model = new MySimulatorModel(client);
            controlsVM = new SimulatorControlsViewModel(model);
            dashboardVM = new SimulatorDashboardViewModel(model);
            mapVM = new SimulatorMapViewModel(model);
        }
    }
}
