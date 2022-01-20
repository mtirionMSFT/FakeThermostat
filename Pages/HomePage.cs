using FakeThermostat.Models;
using FakeThermostat.UI;
using nanoFramework.Presentation.Media;
using System.Threading;

namespace FakeThermostat.Pages
{
    public class HomePage : BasePage
    {
        private Timer _timer;

        public override void Load()
        {
            _timer = new Timer(ShowValues, null, 0, 5000);

            App.Current.LeftButton.Press += LeftButton_Click;
            App.Current.MiddleButton.Press += MiddleButton_Click;
            App.Current.RightButton.Press += RightButton_Click;
        }

        public override void Unload()
        {
            _timer.Dispose();

            App.Current.LeftButton.Press -= LeftButton_Click;
            App.Current.MiddleButton.Press -= MiddleButton_Click;
            App.Current.RightButton.Press -= RightButton_Click;
        }

        private void RightButton_Click(object sender, System.EventArgs e)
        {
            Frame.NavigateTo(new PreferredPage());
        }

        private void MiddleButton_Click(object sender, System.EventArgs e)
        {
            Frame.NavigateTo(new PreferredPage());
        }

        private void LeftButton_Click(object sender, System.EventArgs e)
        {
            Frame.NavigateTo(new PreferredPage());
        }

        private void ShowValues(object state)
        {
            Bmp280Data data = App.Current.Bmp280.GetData();

            App.Current.Screen.Clear();

            App.Current.Screen.Normal.DisplayText(
                "CURRENT", 
                Color.Yellow, 
                HorizontalTextAlignment.Center, VerticalTextAlignment.Top, 
                0, 20);

            App.Current.Screen.Large.DisplayText(
                data.Temperature.ToString("F1"), 
                Color.White, 
                HorizontalTextAlignment.Center, VerticalTextAlignment.Center);
        }
    }
}
