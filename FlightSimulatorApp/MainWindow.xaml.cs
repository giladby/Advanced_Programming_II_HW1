using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Configuration;
using Microsoft.Maps.MapControl.WPF;
using System.Windows.Shell;
using System.Windows.Shapes;


namespace FlightSimulatorApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private bool connected;
        private bool roadViewFlag;
        private bool isFlightRoute;
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
            isFlightRoute = true;

            initFlightRoute();
        }

        private void connectedMode()
        {
            connected = true;
            connectButton.Content = "DISCONNECT";
            myControls.connectedMode();
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

        private void disconnectedMode()
        {
            connected = false;
            connectButton.Content = "CONNECT";
            myControls.disconnectedMode();
            deleteFlightRoute();
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
            deleteFlightRoute();
            if (connected)
            {
                (Application.Current as App).controlsVM.disconnect();
            } else
            {
                (Application.Current as App).controlsVM.connect(ipBox.Text, int.Parse(portBox.Text));
            }
        }

        private void statusBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            string status = statusBox.Text;
            if (MyStatus.isErrorStatus(status))
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
                connectedMode();
                return;
            }
            if (status == MyStatus.disconnectedStatus || status == MyStatus.simulatorDisconnectedStatus || status == MyStatus.notConnectedStatus)
            {
                disconnectedMode();
                return;
            }
            if (status == MyStatus.startLatitudeErrorStatus || status == MyStatus.startLongitudeErrorStatus)
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
            } else
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

        private void initFlightRoute()
        {
            flightRoute = new MapPolyline();
            flightRoute.Stroke = new SolidColorBrush(Colors.Blue);
            flightRoute.Locations = new LocationCollection();
            flightRoute.StrokeDashArray.Add(5);
            flightRoute.StrokeDashArray.Add(2);
            flightRoute.StrokeThickness = 2;
            flightRoute.Opacity = 1;
            myMap.Children.Add(flightRoute);
            Panel.SetZIndex(airplane, Panel.GetZIndex(flightRoute) + 1);
            if (connected && MapLayer.GetPosition(airplane) != null)
            {
                flightRoute.Locations.Add(MapLayer.GetPosition(airplane));
            }
        }

        private void routeCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            isFlightRoute = true;
            if (flightRoute != null)
            {
                flightRoute.Visibility = Visibility.Visible;
            }
        }

        private void routeCheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            isFlightRoute = false;
            if (flightRoute != null)
            {
                flightRoute.Visibility = Visibility.Collapsed;
            }
        }

        private void deleteRouteButton_Click(object sender, RoutedEventArgs e)
        {
            deleteFlightRoute();
        }
        private void deleteFlightRoute()
        {
            myMap.Children.Remove(flightRoute);
            initFlightRoute();
            if (!isFlightRoute)
            {
                if (flightRoute != null)
                {
                    flightRoute.Visibility = Visibility.Collapsed;
                }
            }
        }
    }
}
