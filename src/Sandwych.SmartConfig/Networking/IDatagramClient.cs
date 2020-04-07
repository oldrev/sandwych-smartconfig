using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Sandwych.SmartConfig.Networking
{
    public interface IDatagramClient : IDisposable
    {
        void Bind(IPEndPoint localEndPoint);
        Task SendAsync(byte[] datagram, int bytes, IPEndPoint target);

        Task<DatagramReceiveResult> ReceiveAsync();
    }
}
