using System;
using System.Collections.Generic;
using System.Net;
using System.Net.NetworkInformation;
using System.Text;

namespace Sandwych.SmartConfig
{
    public class SmartConfigDevice : ISmartConfigDevice
    {
        public PhysicalAddress MacAddress { get; }
        public IPAddress IPAddress { get; }

        public SmartConfigDevice(PhysicalAddress mac, IPAddress ip)
        {
            this.MacAddress = mac;
            this.IPAddress = ip;
        }
    }
}
