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
        MapPolyline flightRoute;

        public MainWindow()
        {
            InitializeComponent();
            DataContext = new
            {
                (Application.Current as App).controlsVM,
                (Application.Current as App).mapVM,
                (Application.Current as App).dashboardVM
            };
            myMap.Focus();
            ipBox.Text = ConfigurationManager.AppSettings.Get("ip");
            portBox.Text = ConfigurationManager.AppSettings.Get("port");
            (Application.Current as App).dashboardVM.VM_Status = MyStatus.notConnectedStatus;
            connected = false;
            roadViewFlag = true;
            InitFlightRoute();
        }

        private void ConnectedMode()
        {
            connected = true;
            connectButton.Content = "DISCONNECT";
            myControls.ConnectedMode();
            ipBox.IsEnabled = false;
            portBox.IsEnabled = false;
            controlsView.Visibility = Visibility.Collapsed;
            controlsTextBox.Visibility = Visibility.Collapsed;
            airplane.Visibility = Visibility.Visible;
            planeBoxView.Visibility = Visibility.Collapsed;
            planeViewText.Visibility = Visibility.Collapsed;
            centerButton.IsEnabled = true;
            planeColorBox.IsEnabled = true;
            planeColorBox.SelectedItem = blackColor;
            routeCheckBox.IsEnabled = true;
            deleteRouteButton.IsEnabled = true;
        }

        private void DisconnectedMode()
        {
            connected = false;
            connectButton.Content = "CONNECT";
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
        }

        private void connectButton_Click(object sender, RoutedEventArgs e)
        {
            DeleteFlightRoute();
            if (connected)
            {
                (Application.Current as App).controlsVM.Disconnect();
            }
            else
            {
                (Application.Current as App).controlsVM.Connect(ipBox.Text, int.Parse(portBox.Text));
            }
        }

        private void statusBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            string status = statusBox.Text;
            if (MyStatus.IsErrorStatus(status))
            {
                statusBox.Foreground = new SolidColorBrush(Colors.Red);
            }
            else
            {
                statusBox.Foreground = new SolidColorBrush(Colors.DarkOrange);
            }
            if (status == MyStatus.connectedStatus)
            {
                statusBox.Foreground = new SolidColorBrush(Colors.LimeGreen);
                ConnectedMode();
                return;
            }
            if ((status == MyStatus.disconnectedStatus) || (status == MyStatus.simulatorDisconnectedStatus)
                || (status == MyStatus.notConnectedStatus))
            {
                DisconnectedMode();
                return;
            }
            if ((status == MyStatus.startLatitudeErrorStatus) || (status == MyStatus.startLongitudeErrorStatus))
            {
                airplane.Visibility = Visibility.Collapsed;
                return;
            }
        }

        private void mapViewButton_Click(object sender, RoutedEventArgs e)
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

        private void centerButton_Click(object sender, RoutedEventArgs e)
        {
            myMap.ZoomLevel = 6;
            myMap.Center = MapLayer.GetPosition(airplane);
        }

        private void planeColorBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (planeColorBox.SelectedItem != null)
            {
                string color = planeColorBox.SelectedItem.ToString().Split(' ').Last();
                string imagePath = "Resources/" + color + "_airplane.png";
                plane.Source = new BitmapImage(new Uri(imagePath, UriKind.Relative));
            }
        }

        private void locationBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            flightRoute.Locations.Add(MapLayer.GetPosition(airplane));
        }

        private void InitFlightRoute()
        {
            flightRoute = new MapPolyline() { Stroke = new SolidColorBrush(Colors.Blue), Locations = new LocationCollection(), StrokeThickness = 2 };
            flightRoute.StrokeDashArray.Add(5);
            flightRoute.StrokeDashArray.Add(2);
            myMap.Children.Add(flightRoute);
            Panel.SetZIndex(airplane, Panel.GetZIndex(flightRoute) + 1);
        }

        private void routeCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            if (flightRoute != null)
            {
                flightRoute.Visibility = Visibility.Visible;
            }
        }

        private void routeCheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            if (flightRoute != null)
            {
                flightRoute.Visibility = Visibility.Collapsed;
            }
        }

        private void deleteRouteButton_Click(object sender, RoutedEventArgs e)
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
