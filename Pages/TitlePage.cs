using FakeThermostat.UI;
using nanoFramework.Presentation.Media;
using System.Threading;

namespace FakeThermostat.Pages
{
    public class TitlePage : BasePage
    {
        public override void Load()
        {
            App.Current.Screen.Clear();

            App.Current.Screen.Large.DisplayText(
                $"FAKE", 
                Color.Yellow, 
                HorizontalTextAlignment.Center, VerticalTextAlignment.Bottom, 
                0, 0, 0, 
                (ushort)(App.Current.Screen.Height / 2));

            App.Current.Screen.Normal.DisplayText(
                $"THERMOSTAT", 
                Color.White, 
                HorizontalTextAlignment.Center, VerticalTextAlignment.Top, 
                0, (ushort)(App.Current.Screen.Height / 2), 0, 0);

            Thread.Sleep(3000);

            Frame.NavigateTo(new WifiPage());
        }

        public override void Unload()
        {
        }
    }
}
