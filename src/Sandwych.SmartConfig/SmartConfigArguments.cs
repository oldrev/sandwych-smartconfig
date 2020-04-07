using System;
using System.Collections.Generic;
using System.Net;
using System.Net.NetworkInformation;
using System.Text;

namespace Sandwych.SmartConfig
{
    public class SmartConfigArguments
    {
        public string Password { get; set; }
        public string Ssid { get; set; }
        public PhysicalAddress Bssid { get; set; }
        public IPAddress LocalAddress { get; set; }
        public bool? IsHiddenSsid { get; set; } = null;
    }
}
