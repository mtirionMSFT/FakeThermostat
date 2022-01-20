using FakeThermostat.Models;
using Iot.Device.Bmxx80;
using nanoFramework.Hardware.Esp32;
using System;
using System.Device.I2c;
using System.Threading;

namespace FakeThermostat
{
    public class SensorBmp280 : IDisposable
    {
        private readonly Bmp280 _sensor;

        public SensorBmp280(int busId, int pinData, int pinClock)
        {
            Configuration.SetPinFunction(pinData, DeviceFunction.I2C1_DATA);
            Configuration.SetPinFunction(pinClock, DeviceFunction.I2C1_CLOCK);

            I2cConnectionSettings i2cSettings = new(busId, Bmp280.DefaultI2cAddress);
            I2cDevice i2cDevice = I2cDevice.Create(i2cSettings);
            _sensor = new Bmp280(i2cDevice);
        }

        public Bmp280Data GetData()
        {
            _sensor.TemperatureSampling = Sampling.LowPower;
            _sensor.PressureSampling = Sampling.UltraHighResolution;
            var result = _sensor.Read();
            return new Bmp280Data { Temperature = result.Temperature.DegreesCelsius, Pressure = result.Pressure.Hectopascals };
        }

        public void Dispose()
        {
            _sensor.Dispose();
        }
    }
}
