using FakeThermostat.Pages;
using System;

namespace FakeThermostat.UI
{
    public class Frame
    {
        private BasePage CurrentPage = null;

        public void NavigateTo(BasePage page)
        {
            if (CurrentPage != null)
            {
                CurrentPage.Dispose();
            }

            CurrentPage = page;
            CurrentPage.Load();
        }
    }
}
