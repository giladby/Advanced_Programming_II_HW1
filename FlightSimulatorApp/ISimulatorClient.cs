
namespace FlightSimulatorApp
{
    public interface ISimulatorClient
    {
        string Connect(string ip, int port);
        void Disconnect();
        string Send(string data);
        string Recieve();
    }
}
