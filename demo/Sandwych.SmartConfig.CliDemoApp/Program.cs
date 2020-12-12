using System;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Threading.Tasks;

using Sandwych.SmartConfig;
using Sandwych.SmartConfig.Esptouch;

namespace Sandwych.SmartConfig.CliDemoApp
{
    class Program
    {
        private static NetworkInterface FindFirstWifiInterfaceOrDefault()
        {
            var adapters = NetworkInterface.GetAllNetworkInterfaces();
            return adapters.Where(
                x => x.NetworkInterfaceType == NetworkInterfaceType.Wireless80211
                && x.OperationalStatus == OperationalStatus.Up
                && !x.IsReceiveOnly
            ).FirstOrDefault();
        }

        private static IPAddress GetIPv4AddressOrDefault(NetworkInterface ni)
        {
            return ni.GetIPProperties()
                    .UnicastAddresses
                    .Where(x => x.Address.AddressFamily == AddressFamily.InterNetwork)
                    .Select(x => x.Address)
                    .FirstOrDefault();
        }

        static async Task<int> Main(string[] args)
        {
            Console.WriteLine("******** ESPTouch SmartConfig Demo/Utility ********");
            if (args.Length != 3)
            {
                ShowUsage();
                return -1;
            }

            var wifiInterface = FindFirstWifiInterfaceOrDefault();
            if (wifiInterface == null)
            {
                Console.WriteLine("Cannot find any available WiFi adapter.");
                return -1;
            }
            Console.WriteLine("WiFi interface: {0}", wifiInterface.Name);

            var localAddress = GetIPv4AddressOrDefault(wifiInterface);
            if(localAddress == null)
            {
                Console.WriteLine("Cannot find IPv4 address for WiFi interface: {0}", wifiInterface.Name);
                return -1;
            }
            Console.WriteLine("Local address: {0}", localAddress);

            var provider = new EspSmartConfigProvider();
            var ctx = provider.CreateContext();

            ctx.DeviceDiscoveredEvent += (s, e) =>
            {
                Console.WriteLine("Found device: IP={0}    MAC={1}", e.Device.IPAddress, e.Device.MacAddress);
            };

            var scArgs = new SmartConfigArguments()
            {
                Ssid = args[0],
                Bssid = PhysicalAddress.Parse(args[1].ToUpperInvariant().Replace(':', '-')),
                Password = args[2],
                LocalAddress = localAddress
            };

            // Do the SmartConfig job
            using (var job = new SmartConfigJob(TimeSpan.FromSeconds(20))) // Set the timeout to 20 seconds
            {
                job.Elapsed += Job_Elapsed;

                await job.ExecuteAsync(ctx, scArgs);
            }

            Console.WriteLine("SmartConfig finished.");
            return 0;
        }

        private static void ShowUsage()
        {
            Console.WriteLine("\nUSAGE:");
            Console.WriteLine("sccli.exe <AP SSID> <AP BSSID> <AP Password>");
            Console.WriteLine("\tAP SSID:\tThe SSID of your WiFi AP.");
            Console.WriteLine("\tAP BSSID:\tThe BSSID(MAC) of your WiFi AP, like '10-10-10-10-10-10' or '10:10:10:10:10:10'");
            Console.WriteLine("\tAP Password:\tThe password of your WiFi AP.");
            Console.WriteLine("\nTIPS:");
            Console.WriteLine("\tOn Windows you can get BSSID by using command 'netsh wlan show interfaces'");
        }

        private static void Job_Elapsed(object sender, SmartConfigTimerEventArgs e)
        {
            Console.WriteLine("Doing SmartConfig, Time remaining: {0}", e.LeftTime);
        }
    }
}
