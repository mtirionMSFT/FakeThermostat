using FakeThermostat.UI;
using nanoFramework.Presentation.Media;
using System.Threading;

namespace FakeThermostat.Pages
{
    public class WifiPage : BasePage
    {
        public override void Load()
        {
            App.Current.Screen.Clear();

            App.Current.Screen.Normal.DisplayText(
                $"Connecting WIFI", 
                Color.Yellow, 
                HorizontalTextAlignment.Center, VerticalTextAlignment.Bottom, 
                0, 0, 0, (ushort)(App.Current.Screen.Height / 2));
            App.Current.Screen.Normal.DisplayText(
                $"{App.WIFI_SSID}", 
                Color.White, 
                HorizontalTextAlignment.Center, VerticalTextAlignment.Top, 
                0, (ushort)(App.Current.Screen.Height / 2), 0, 0);

            App.Current.Connect();

            App.Current.Screen.Clear();
            App.Current.Screen.Normal.DisplayText(
                $"Connected to", 
                Color.Yellow, 
                HorizontalTextAlignment.Center, VerticalTextAlignment.Bottom, 
                0, 0, 0, (ushort)(App.Current.Screen.Height / 2));
            App.Current.Screen.Normal.DisplayText(
                $"{App.WIFI_SSID}", 
                Color.White, 
                HorizontalTextAlignment.Center, VerticalTextAlignment.Top, 
                0, (ushort)(App.Current.Screen.Height / 2), 0, 0);

            Thread.Sleep(3000);

            Frame.NavigateTo(new HomePage());
        }

        public override void Unload()
        {
        }
    }
}
