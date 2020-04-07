using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Sandwych.SmartConfig.Networking
{
    public class DefaultDatagramClient : UdpClient, IDatagramClient
    {
        public DefaultDatagramClient() : base(AddressFamily.InterNetwork)
        {
            this.EnableBroadcast = true;
        }

        public void Bind(IPEndPoint localEndPoint)
        {
            this.Client.Bind(localEndPoint);
        }

        async Task IDatagramClient.SendAsync(byte[] datagram, int bytes, IPEndPoint target)
        {
            await this.SendAsync(datagram, bytes, target);
        }

        async Task<DatagramReceiveResult> IDatagramClient.ReceiveAsync()
        {
            var result = await this.ReceiveAsync();
            return new DatagramReceiveResult(result.Buffer, result.RemoteEndPoint);
        }
    }
}
