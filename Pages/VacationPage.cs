using FakeThermostat.UI;
using nanoFramework.Presentation.Media;
using System;

namespace FakeThermostat.Pages
{
    public class VacationPage : BasePage
    {
        public override void Load()
        {
            Show();

            App.Current.LeftButton.Press += LeftButton_Click;
            App.Current.MiddleButton.Press += MiddleButton_Click;
            App.Current.RightButton.Press += RightButton_Click;

            App.Current.MiddleButton.IsHoldingEnabled = true;
            App.Current.MiddleButton.Holding += MiddleButton_Holding;
        }

        private void MiddleButton_Holding(object sender, Iot.Device.Button.ButtonHoldingEventArgs e)
        {
            if (e.HoldingState == Iot.Device.Button.ButtonHoldingState.Started)
            {
                App.Current.PreferredTemperature = App.Current.VacationTemperature;
            }
        }

        public override void Unload()
        {
            App.Current.LeftButton.Press -= LeftButton_Click;
            App.Current.MiddleButton.Press -= MiddleButton_Click;
            App.Current.RightButton.Press -= RightButton_Click;
        }

        private void RightButton_Click(object sender, EventArgs e)
        {
            App.Current.VacationTemperature += 0.5;
            Show();
        }

        private void MiddleButton_Click(object sender, EventArgs e)
        {
            Frame.NavigateTo(new HomePage());
        }

        private void LeftButton_Click(object sender, EventArgs e)
        {
            App.Current.VacationTemperature -= 0.5;
            Show();
        }

        private void Show()
        {
            App.Current.Screen.Clear();
            App.Current.Screen.Normal.DisplayText(
                "VACATION",
                Color.Yellow,
                HorizontalTextAlignment.Center, VerticalTextAlignment.Top,
                0, 20);

            App.Current.Screen.Large.DisplayText(
                App.Current.VacationTemperature.ToString("F1"),
                Color.White,
                HorizontalTextAlignment.Center, VerticalTextAlignment.Center);
        }
    }
}
