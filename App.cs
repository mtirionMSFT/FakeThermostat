using FakeThermostat.Pages;
using FakeThermostat.Services;
using FakeThermostat.UI;
using Iot.Device.Button;
using nanoFramework.Networking;
using System.Diagnostics;
using System.Threading;

namespace FakeThermostat
{
    public class App
    {
        // configuration
        #region Wifi config
        public const string WIFI_SSID = "clowntjecopernicus";
        public const string WIFI_PASSWD = "TIR@home!2";
        #endregion

        #region Azure Iot Hub config
        public const string DEVICE_ID = "FakeThermostat";
        public const  string IOT_BROKER_ADDRESS = "hub-tir-iot.azure-devices.net";
        //public const string IOT_BROKER_ADDRESS = "TIRPI4";

        public const string EDGE_CONNECTION_STRING = "HostName=hub-tir-iot.azure-devices.net;DeviceId=FakeThermostat;SharedAccessKey=TYwOwz5X9nXKWh99AeMxlmWhrFo8VylMNt7fYZq85kY=";

        // X509Certificate password, certificates in PEM format and key
        public const string CERTIFICATE_PASSWORD = "p@ssw0rd!";

        public const string PUBLIC_CERTIFICATE =
            @"-----BEGIN CERTIFICATE-----
            MIIDhzCCAm+gAwIBAgIRAI5YYAVsaaTiJYaE8mIQ5p4wDQYJKoZIhvcNAQELBQAw
            FzEVMBMGA1UEAwwMVGVzdCBSb290IENBMB4XDTIxMDkyNzE0MDc0OFoXDTMxMDky
            NTE0MDc0OFowcDELMAkGA1UEBhMCVUsxDzANBgNVBAgMBkxvbmRvbjEQMA4GA1UE
            CgwHQ29udG9zbzELMAkGA1UECwwCSVQxEDAOBgNVBAMMB001U3RhY2sxHzAdBgkq
            hkiG9w0BCQEWEGluZm9AY29udG9zby5jb20wggEiMA0GCSqGSIb3DQEBAQUAA4IB
            DwAwggEKAoIBAQCl9GecGCK2ftay1Fg4sdDqXdxvw5z5wQEcO3ZE4ahIrtq7fnbI
            rFBDK+0mGeqW3gpZWsn9hz3lkT0PKuPN6i907Sib7B3Y8rc4IeNAOFqupF1fScO0
            G/sXmiAZPQrvXXygK/X6XU9C/Ao2ZUpJ/UihMg6zQbwpTc3eB35PXABh1p61Vvi9
            FMxj/XjXU0hj7sRSVgE1beSbaj1Gr6cky6EK06AyXlOrYyEDIk/EhkMWf2nGqr8t
            u5hh6JMiuqJwggYy9wGs1E1hmxWdT9eFPvkyCg1uFvjYPK0TPDvn4Zp987+77DML
            QJqrpYrKa6uzUMI/YBadOb7vB5qwDEwQCg7lAgMBAAGjdTBzMB8GA1UdIwQYMBaA
            FJ93ssB299+ggvh1L3TRd9bQ4aMiMAwGA1UdEwEB/wQCMAAwEwYDVR0lBAwwCgYI
            KwYBBQUHAwIwDgYDVR0PAQH/BAQDAgeAMB0GA1UdDgQWBBQITwHavFJr/0+1s9O3
            5lrZjCLhKjANBgkqhkiG9w0BAQsFAAOCAQEAk279gtNZJyYAoNp6/m9fc/zc+tkY
            2NOiVx692nxM1LChP7kqM8hSf2X/Z4mGYTlrQU9Z2FVn9y239PPy6B+p5TYI/nu0
            VVZIsPQPbLemo+rJtCuxa2Touk+aRxFEh3wg6k1CpwT2z7PrbQP5dsqOpcfYn2Bx
            OAvTm0fYXaC4w/tgRFj3xqEQXVSKRzbIlb3Erqbo+HhJ5asjEXa+aPgeu07v4JY5
            GQst976bgSOLPDj/hfUETqSL152+zDPUOMYwdpB1tp8TQbIXPO90fIV2kHOaTMX6
            5ddUaPhdYiPFSPv7768aYAhOvCinAxiEVwlIgM3SHPEa8/8KO/+fY/wMIg==
            -----END CERTIFICATE-----";

        public const string PRIVATE_KEY =
            @"-----BEGIN PRIVATE KEY-----
            MIIEvQIBADANBgkqhkiG9w0BAQEFAASCBKcwggSjAgEAAoIBAQCl9GecGCK2ftay
            1Fg4sdDqXdxvw5z5wQEcO3ZE4ahIrtq7fnbIrFBDK+0mGeqW3gpZWsn9hz3lkT0P
            KuPN6i907Sib7B3Y8rc4IeNAOFqupF1fScO0G/sXmiAZPQrvXXygK/X6XU9C/Ao2
            ZUpJ/UihMg6zQbwpTc3eB35PXABh1p61Vvi9FMxj/XjXU0hj7sRSVgE1beSbaj1G
            r6cky6EK06AyXlOrYyEDIk/EhkMWf2nGqr8tu5hh6JMiuqJwggYy9wGs1E1hmxWd
            T9eFPvkyCg1uFvjYPK0TPDvn4Zp987+77DMLQJqrpYrKa6uzUMI/YBadOb7vB5qw
            DEwQCg7lAgMBAAECggEAORJGF25NcclI+JMCC+1K1W6WAnGZKGLxiM4CLEoWX3iS
            jGw/WV+48oDGu5WqEVwm3vfUAzZhWPOLtg2f7g6dZ26vyn92/EbsL4en6Uv4E5s+
            6Sgq/2OoBEPiygsuGYdi9opnu0Qfn/+hW3eWBr/uHFFksMzocqPpKnQVQiF3mC1y
            ++Wr1kZLuRxIJgRRr/lWrf+szhjiDKKuB/wUS7hzgdbqmndcBI1MQ45UmtoAqVm4
            NHHNBngSczlqmCYBjRrSryRwla1kvxQEu4lzs1W5SXHg6nImauuA35Y/I7lHpD6k
            GT0CzejF4ekiwX179IK6RQAfVW2xG+VuLCT5Jns4+QKBgQDQgi51PBhuzFbAAgq8
            It7/q6twfuRYPDZ7YQ1kp0bED4GoEG9JDh3R3c1GZy4KPqF5kzajfP4g7VoT8z/h
            V9D55w8S202CE5Mi7LNJk/ceIDJwWJyKma+bW7G1C5gl0PgKPajLB8Wu2qc0kVSU
            Ezk4Yqk5m20VrgtoCBcrgoq1OwKBgQDLwPlmuMcYBDGlYkLNxGaV+YjYBl55kUkJ
            LQ4vl4WNsgecYoTmuOFA3qb5xGfVk5MiuN8ctahC09tUErxGKT+IWwAJ9tQSvrjq
            xEKDHsRckS565A5mjcRMCtUTjGLFGZdFudxQsf+XhyDb/1cjamdKKo/JM+bktZvr
            8TrsIK2KXwKBgQCDmebgr7FsHSSTw7Yq0IeXLy7hrfVJi5eHf9YINVwA2ximjtju
            by57C1dInE7+wFECftv5jCaJecVk2h7zh42qbR+iczAUbW5smKEAS5epOAEdz4/e
            GuovUEx1TIAXPjGPpSMoIhuvJOprz15mp0tyPzM33NWY3WTtoE2fdKE/iQKBgDrc
            y/AuD25Tv9fsdConxA8toKfv5xktrLDW8FM1beLKfMJj/8r7vC9WY1yirfCYipgs
            WmBb5nkv5Rv7saJ/RhhpWbCHOysTKC7CdgiVOdsYIhpkifh8minxsy7LjrksNHRz
            Rj+VvKYU5pxHvu+/TImzlAhnUxvdj2bxMLmIkzn3AoGAWHeMvvMmG80cizzjrWe9
            Hrrz/4WDhKGQOGqi9U++ZMkN/hdpUKnRr0Lyi7YtZfAa8A2drVkUQPZGmR+77nGe
            x1J3iWvI7yF9pkjCb/GgU8ZqHdlsc/AKriYPJQWoDA5x06/Q5n6q5IYzWeG4ACtg
            EAfjsNNafMs562Bi4B19eKw=
            -----END PRIVATE KEY-----";

        // Azure RootCA!
        public const string AZURE_ROOT_CA =
            @"-----BEGIN CERTIFICATE-----
            MIIDdzCCAl+gAwIBAgIEAgAAuTANBgkqhkiG9w0BAQUFADBaMQswCQYDVQQGEwJJ
            RTESMBAGA1UEChMJQmFsdGltb3JlMRMwEQYDVQQLEwpDeWJlclRydXN0MSIwIAYD
            VQQDExlCYWx0aW1vcmUgQ3liZXJUcnVzdCBSb290MB4XDTAwMDUxMjE4NDYwMFoX
            DTI1MDUxMjIzNTkwMFowWjELMAkGA1UEBhMCSUUxEjAQBgNVBAoTCUJhbHRpbW9y
            ZTETMBEGA1UECxMKQ3liZXJUcnVzdDEiMCAGA1UEAxMZQmFsdGltb3JlIEN5YmVy
            VHJ1c3QgUm9vdDCCASIwDQYJKoZIhvcNAQEBBQADggEPADCCAQoCggEBAKMEuyKr
            mD1X6CZymrV51Cni4eiVgLGw41uOKymaZN+hXe2wCQVt2yguzmKiYv60iNoS6zjr
            IZ3AQSsBUnuId9Mcj8e6uYi1agnnc+gRQKfRzMpijS3ljwumUNKoUMMo6vWrJYeK
            mpYcqWe4PwzV9/lSEy/CG9VwcPCPwBLKBsua4dnKM3p31vjsufFoREJIE9LAwqSu
            XmD+tqYF/LTdB1kC1FkYmGP1pWPgkAx9XbIGevOF6uvUA65ehD5f/xXtabz5OTZy
            dc93Uk3zyZAsuT3lySNTPx8kmCFcB5kpvcY67Oduhjprl3RjM71oGDHweI12v/ye
            jl0qhqdNkNwnGjkCAwEAAaNFMEMwHQYDVR0OBBYEFOWdWTCCR1jMrPoIVDaGezq1
            BE3wMBIGA1UdEwEB/wQIMAYBAf8CAQMwDgYDVR0PAQH/BAQDAgEGMA0GCSqGSIb3
            DQEBBQUAA4IBAQCFDF2O5G9RaEIFoN27TyclhAO992T9Ldcw46QQF+vaKSm2eT92
            9hkTI7gQCvlYpNRhcL0EYWoSihfVCr3FvDB81ukMJY2GQE/szKN+OMY3EU/t3Wgx
            jkzSswF07r51XgdIGn9w/xZchMB5hbgF/X++ZRGjD8ACtPhSNzkE1akxehi/oCr0
            Epn3o0WC4zxe9Z2etciefC7IpJ5OCBRLbf1wbWsaY71k5h+3zvDyny67G7fyUIhz
            ksLi4xaNmjICq44Y3ekQEe5+NauQrz4wlHrQMz2nZQ/1/I6eYs9HRCwBXbsdtTLS
            R9I4LtD+gdwyah617jzV/OeBHRnDJELqYzmp
            -----END CERTIFICATE-----";
        #endregion

        // instance of this class (singleton)
        public static App Current;

        // all devices
        public Screen Screen { get; private set; }
        public SensorBmp280 Bmp280 { get; private set; }
        public GpioButton LeftButton { get; set; }
        public GpioButton MiddleButton { get; set; }
        public GpioButton RightButton { get; set; }

        // all services
        private AzureIoTHubService IotHubService { get; set; }

        // Main frame
        public Frame Frame { get; private set; } = new Frame();

        // settings
        public double PreferredTemperature { get; set; } = 20.0;
        public double VacationTemperature { get; set; } = 15.0;
        public double ComfortTemperature { get; set; } = 21.0;

        /// <summary>
        /// Constructor of the <see cref="App"/> class.
        /// </summary>
        private App()
        {
            // initialize devices
            Screen = new Screen(320, 240);
            Bmp280 = new SensorBmp280(1, 21, 22);
            LeftButton = new GpioButton(39);
            MiddleButton = new GpioButton(38);
            RightButton = new GpioButton(37);
        }

        /// <summary>
        /// Initialize the Current application.
        /// </summary>
        public static void Initialize()
        {
            Current = new App();
        }

        /// <summary>
        /// Start the actual logic of the app.
        /// </summary>
        public void Run()
        {
            // initialize services
            IotHubService = new AzureIoTHubService();
            IotHubService.Start();

            // show title page            
            Frame.NavigateTo(new TitlePage());
        }

        #region Wifi Connect
        public bool Connect(int sleepTime = 60000)
        {
            Debug.WriteLine($"Connecting to {WIFI_SSID}");

            // As we are using TLS, we need a valid date & time
            // We will wait maximum 1 minute to get connected and have a valid date
            var success = NetworkHelper.ConnectWifiDhcp(WIFI_SSID, WIFI_PASSWD, setDateTime: true, token: new CancellationTokenSource(sleepTime).Token);
            if (!success)
            {
                Debug.WriteLine($"Can't connect to wifi: {NetworkHelper.ConnectionError.Error}");
                if (NetworkHelper.ConnectionError.Exception != null)
                {
                    Debug.WriteLine($"NetworkHelper.ConnectionError.Exception");
                }
            }

            Debug.WriteLine($"Connected to {WIFI_SSID}");
            return success;
        }
        #endregion
    }
}
