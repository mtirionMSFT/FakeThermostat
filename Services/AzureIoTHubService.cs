using FakeThermostat.Models;
using nanoFramework.Azure.Devices.Client;
using nanoFramework.M2Mqtt;
using System.Diagnostics;
using System.Security.Cryptography.X509Certificates;
using System.Threading;

namespace FakeThermostat.Services
{
    public class AzureIoTHubService
    {
        private DeviceClient _client;
        private Timer _timer;

        public AzureIoTHubService()
        {
            // Create an X.509 certificate object and create device client for Azure IoT Hub
            X509Certificate2 cert = new X509Certificate2(
                App.AZURE_ROOT_CA, App.PRIVATE_KEY, App.CERTIFICATE_PASSWORD);
            _client = new DeviceClient(
                App.IOT_BROKER_ADDRESS, App.DEVICE_ID, cert, azureCert: new X509Certificate(App.AZURE_ROOT_CA));

            // using connection string (SAS token)
            //MqttClient client = new MqttClient("TIRPI4", 1883, true, new X509Certificate(App.AZURE_ROOT_CA), null, MqttSslProtocols.TLSv1_2);
            //client.Connect("FakeThermostat");

            _client = new DeviceClient(App.IOT_BROKER_ADDRESS, App.DEVICE_ID, App.EDGE_CONNECTION_STRING);

            bool isOpen = _client.Open();
            Debug.WriteLine($"Connection to {App.IOT_BROKER_ADDRESS} isOpen={isOpen}");
        }

        public void Start(int interval = 60000)
        {
            _timer?.Dispose();
            _timer = new Timer(SendTelemetry, null, 0, interval);
        }

        public void Stop()
        {
            _timer?.Dispose();
            _timer = null;
        }

        private void SendTelemetry(object state)
        {
            Bmp280Data data = App.Current.Bmp280.GetData();

            // send to Iot Hub
            string message = $"{{\"Temperature\":{data.Temperature},\"Pressure\":{data.Pressure},\"DeviceID\":\"{App.DEVICE_ID}\"}}";
            _client.SendMessage(message);
            Debug.WriteLine($"Send Iot Hub: {message}");
        }
    }
}
