using System.Threading;

namespace FakeThermostat
{
    public class Program
    {

        public static void Main()
        {
            App.Initialize();

            App.Current.Run();

            Thread.Sleep(Timeout.Infinite);
        }
    }
}
