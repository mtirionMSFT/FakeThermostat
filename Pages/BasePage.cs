using FakeThermostat.UI;
using System;

namespace FakeThermostat.Pages
{
    public abstract class BasePage : IDisposable
    {
        protected Frame Frame{ get { return App.Current.Frame; } }

        public abstract void Load();
        public abstract void Unload();

        public void Dispose()
        {
            Unload();
        }
    }
}
