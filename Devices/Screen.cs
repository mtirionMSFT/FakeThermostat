using FakeThermostat.UI;
using nanoFramework.Hardware.Esp32;
using nanoFramework.UI;
using System.Diagnostics;
using Windows.Devices.Pwm;

namespace FakeThermostat
{
    public class Screen
    {
        public ushort Width { get; set; }
        public ushort Height { get; set; }

        public ushort HorizontalBreakOf
        {
            get { return (ushort)(Width - (Width * 0.05)); }
        }

        public TextContext Normal { get; private set; }
        public TextContext Large { get; private set; }

        public Screen(
            ushort width, ushort height, 
            uint buffersize = 0, 
            double frequency = 44100, 
            int pinMiso = 19, int pinMosi = 23, int pinClock = 18, int pinBacklight = 32,
            int chip = 14, int dataCommand = 27, int reset = 33)
        {
            Width = width;
            Height = height;

            Debug.WriteLine($"Initializing screen for width={Width} height={Height} breakof={HorizontalBreakOf}");

            Configuration.SetPinFunction(19, DeviceFunction.SPI1_MISO);
            Configuration.SetPinFunction(23, DeviceFunction.SPI1_MOSI);
            Configuration.SetPinFunction(18, DeviceFunction.SPI1_CLOCK);
            Configuration.SetPinFunction(pinBacklight, DeviceFunction.PWM1);

            DisplayControl.Initialize(
                new SpiConfiguration(1, chip, dataCommand, reset, pinBacklight),
                new ScreenConfiguration(0, 0, width, height), buffersize);

            PwmController pwm = PwmController.GetDefault();
            pwm.SetDesiredFrequency(44100);
            PwmPin pwmPin = pwm.OpenPin(pinBacklight);
            pwmPin.SetActiveDutyCyclePercentage(0.1);
            pwmPin.Start();

            Normal = new TextContext(this, (short)Resource.FontResources.arialregular24);
            Large = new TextContext(this, (short)Resource.FontResources.segoeuiregular72);

            Debug.WriteLine($"Screen initialized");
        }

        public void Clear()
        {
            DisplayControl.Clear();
        }
    }
}
