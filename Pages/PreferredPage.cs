using FakeThermostat.UI;
using nanoFramework.Presentation.Media;
using System;
using System.Threading;

namespace FakeThermostat.Pages
{
    public class PreferredPage : BasePage
    {
        private Timer _timer;

        public override void Load()
        {
            Show();

            App.Current.LeftButton.Press += LeftButton_Click;
            App.Current.MiddleButton.Press += MiddleButton_Click;
            App.Current.RightButton.Press += RightButton_Click;

            SetTimer();
        }

        public override void Unload()
        {
            _timer?.Dispose();
            _timer = null;

            App.Current.LeftButton.Press -= LeftButton_Click;
            App.Current.MiddleButton.Press -= MiddleButton_Click;
            App.Current.RightButton.Press -= RightButton_Click;
        }

        private void SetTimer()
        {
            _timer?.Dispose();
            _timer = new Timer(HandleTimeout, null, 5000, Timeout.Infinite);
        }

        private void HandleTimeout(object state)
        {
            Frame.NavigateTo(new HomePage());
        }

        private void RightButton_Click(object sender, EventArgs e)
        {
            SetTimer();
            App.Current.PreferredTemperature += 0.5;
            Show();
        }

        private void MiddleButton_Click(object sender, EventArgs e)
        {
            Frame.NavigateTo(new ComfortPage());
        }

        private void LeftButton_Click(object sender, EventArgs e)
        {
            SetTimer();
            App.Current.PreferredTemperature -= 0.5;
            Show();
        }

        private void Show()
        {
            App.Current.Screen.Clear();
            App.Current.Screen.Normal.DisplayText(
                "SET",
                Color.Yellow,
                HorizontalTextAlignment.Center, VerticalTextAlignment.Top,
                0, 20);

            App.Current.Screen.Large.DisplayText(
                App.Current.PreferredTemperature.ToString("F1"),
                Color.White,
                HorizontalTextAlignment.Center, VerticalTextAlignment.Center);
        }
    }
}
