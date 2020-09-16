WPF_Project_FlightSimulator

In this project, we built a desktop application using WPF that follows the MVVM pattern principles.
The application connects to a flight simulator, controls it and get live data from it.
The application GUI has a world map showing the airplane location, a dashboard showing the current airplane parameters
and a joystick and sliders to control the movement of the airplane.

These are some additional features we added to the application:
1. When a single error message appears, it will be displayed for 5 seconds
on the status bar and after that the basic "Connected"/"Not Connected" status appears again.
But if a new error message appears whitin those 5 seconds, the timer resets.
2. While the application is not connected to a simulator, we lock all the features that
are supposed to be used only during a connection (e.g the controls), and present a proper message.
3. Each time the airplane moves on the map, we calculate the correct angle of the movement direction,
so that the airplane points to his current destination.
4. A combo box that present a colors menu, and allows changing the airplane color dynamically.
5. A dashed flight route line that is presented on the map and is updated according to the airplane path.
6. A button which turns on/off the flight route line display.
7. A button which deletes the current flight route line.
8. A button which centers and focuses on the current airplane location.
9. A button which switches the map display mode to "aerial view" or "road view".
10. After a connection with the simulator is established, the "connect" button switches to "disconnect",
so now the user can disconnect from the simulator and connect again later.
