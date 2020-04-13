using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Configuration;
using Microsoft.Maps.MapControl.WPF;

namespace FlightSimulatorApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private bool connected;
        private bool roadViewFlag;
        private MapPolyline flightRoute;

        public MainWindow()
        {
            InitializeComponent();
            // Set the DataContext to the 3 ViewModels.
            DataContext = new
            {
                (Application.Current as App).ControlsVM,
                (Application.Current as App).MapVM,
                (Application.Current as App).DashboardVM
            };
            myMap.Focus();
            // Get the IP and port from the App.config file.
            ipBox.Text = ConfigurationManager.AppSettings.Get("ip");
            portBox.Text = ConfigurationManager.AppSettings.Get("port");
            // Set the status to "Not Connected" as a start.
            (Application.Current as App).DashboardVM.VmStatus = MyStatus.NotConnectedStatus;
            connected = false;
            roadViewFlag = true;
            InitFlightRoute();
        }

        // A mode for when the program is connected to a simulator.
        private void ConnectedMode()
        {
            connected = true;
            connectDisconnectButton.Content = "DISCONNECT";
            myControls.ConnectedMode();
            ipBox.IsEnabled = false;
            portBox.IsEnabled = false;
            controlsView.Visibility = Visibility.Collapsed;
            controlsTextBox.Visibility = Visibility.Collapsed;
            planeBoxView.Visibility = Visibility.Collapsed;
            planeViewText.Visibility = Visibility.Collapsed;
            centerButton.IsEnabled = true;
            planeColorBox.IsEnabled = true;
            planeColorBox.SelectedItem = blackColor;
            routeCheckBox.IsEnabled = true;
            deleteRouteButton.IsEnabled = true;
        }

        // A mode for when the program disconnected from a simulator.
        private void DisconnectedMode()
        {
            connected = false;
            connectDisconnectButton.Content = "CONNECT";
            myControls.DisconnectedMode();
            DeleteFlightRoute();
            ipBox.IsEnabled = true;
            portBox.IsEnabled = true;
            controlsView.Visibility = Visibility.Visible;
            controlsTextBox.Visibility = Visibility.Visible;
            airplane.Visibility = Visibility.Collapsed;
            planeViewText.Visibility = Visibility.Visible;
            planeBoxView.Visibility = Visibility.Visible;
            centerButton.IsEnabled = false;
            planeColorBox.IsEnabled = false;
            planeColorBox.SelectedItem = null;
            routeCheckBox.IsEnabled = false;
            deleteRouteButton.IsEnabled = false;
            locationBox.Text = "";
        }

        private void ConnectDisconnectButton_Click(object sender, RoutedEventArgs e)
        {
            // Deletes the old flight route when a simulator is connected or disconnected.
            DeleteFlightRoute();
            if (connected)
            {
                (Application.Current as App).ControlsVM.Disconnect();
            }
            else
            {
                (Application.Current as App).ControlsVM.Connect(ipBox.Text, int.Parse(portBox.Text));
            }
        }

        private void StatusBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            string status = statusBox.Text;
            // Set the status color to red if this is an error, set to orange otherwise.
            if (MyStatus.IsErrorStatus(status))
            {
                statusBox.Foreground = new SolidColorBrush(Colors.Red);
            }
            else
            {
                statusBox.Foreground = new SolidColorBrush(Colors.DarkOrange);
            }
            // Set the status color to green if connected.
            if (status == MyStatus.ConnectedStatus)
            {
                statusBox.Foreground = new SolidColorBrush(Colors.LimeGreen);
                ConnectedMode();
                return;
            }
            // If disconnected.
            if ((status == MyStatus.DisconnectedStatus) || (status == MyStatus.SimulatorDisconnectedStatus)
                || (status == MyStatus.NotConnectedStatus))
            {
                DisconnectedMode();
                return;
            }
            // If got bad latitude or longitude values at the start.
            if ((status == MyStatus.StartLatitudeErrorStatus) || (status == MyStatus.StartLongitudeErrorStatus))
            {
                airplane.Visibility = Visibility.Collapsed;
                return;
            }
        }

        // Change the map appearance.
        private void MapViewButton_Click(object sender, RoutedEventArgs e)
        {
            if (roadViewFlag)
            {
                mapViewButton.Content = "Change To Road View";
                myMap.Mode = new AerialMode(true);
                roadViewFlag = false;
            }
            else
            {
                mapViewButton.Content = "Change To Aerial View";
                myMap.Mode = new RoadMode();
                roadViewFlag = true;
            }
        }

        // Center and zoom on the airplane.
        private void CenterButton_Click(object sender, RoutedEventArgs e)
        {
            myMap.ZoomLevel = 6;
            myMap.Center = MapLayer.GetPosition(airplane);
        }

        // Set the airplane color according to the user selection, by changing the airplane image.
        private void PlaneColorBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (planeColorBox.SelectedItem != null)
            {
                string color = planeColorBox.SelectedItem.ToString().Split(' ').Last();
                string imagePath = "Resources/" + color + "_airplane.png";
                plane.Source = new BitmapImage(new Uri(imagePath, UriKind.Relative));
            }
        }

        // Add a new point to the flight route line.
        private void LocationBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (locationBox.Text == "")
            {
                return;
            }
            if (airplane.Visibility == Visibility.Collapsed)
            {
                airplane.Visibility = Visibility.Visible;
            }
            flightRoute.Locations.Add(MapLayer.GetPosition(airplane));
        }

        // Make the flight route line.
        private void InitFlightRoute()
        {
            flightRoute = new MapPolyline() { Stroke = new SolidColorBrush(Colors.Blue), Locations = new LocationCollection(), StrokeThickness = 2 };
            flightRoute.StrokeDashArray.Add(5);
            flightRoute.StrokeDashArray.Add(2);
            myMap.Children.Add(flightRoute);
            Panel.SetZIndex(airplane, Panel.GetZIndex(flightRoute) + 1);
        }

        private void RouteCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            if (flightRoute != null)
            {
                flightRoute.Visibility = Visibility.Visible;
            }
        }

        private void RouteCheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            if (flightRoute != null)
            {
                flightRoute.Visibility = Visibility.Collapsed;
            }
        }

        private void DeleteRouteButton_Click(object sender, RoutedEventArgs e)
        {
            DeleteFlightRoute();
        }

        private void DeleteFlightRoute()
        {
            flightRoute.Locations.Clear();
            if (connected && MapLayer.GetPosition(airplane) != null)
            {
                flightRoute.Locations.Add(MapLayer.GetPosition(airplane));
            }
        }
    }
}
