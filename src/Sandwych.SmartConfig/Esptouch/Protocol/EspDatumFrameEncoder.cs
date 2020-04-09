using Sandwych.SmartConfig.Protocol;
using Sandwych.SmartConfig.Util;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace Sandwych.SmartConfig.Esptouch.Protocol
{
    public sealed class EspDatumFrameEncoder
    {
        public const int ExtraHeaderLength = 5;
        public const ushort ExtraLength = 40;

        private List<ushort> _framesBuilder = new List<ushort>(128);

        public IEnumerable<ushort> Encode(SmartConfigContext ctx, SmartConfigArguments args)
        {
            // Data = total len(1 byte) + apPwd len(1 byte) + SSID CRC(1 byte) +
            // BSSID CRC(1 byte) + TOTAL XOR(1 byte)+ ipAddress(4 byte) + apPwd + apSsid apPwdLen <=
            // 105 at the moment

            var senderIPAddress = args.LocalAddress.GetAddressBytes();

            var passwordBytes = args.Password != null ? Encoding.ASCII.GetBytes(args.Password) : Constants.EmptyBuffer;

            var ssid = Encoding.UTF8.GetBytes(args.Ssid);
            var ssidCrc8 = Crc8.ComputeOnceOnly(ssid);

            var bssid = args.Bssid?.GetAddressBytes() ?? Constants.EmptyBuffer;
            var bssidCrc8 = Crc8.ComputeOnceOnly(bssid);

            var totalLength = (byte)(ExtraHeaderLength + senderIPAddress.Length + passwordBytes.Length + ssid.Length);

            byte totalXor = ComputeTotalXor(senderIPAddress, passwordBytes, ssid, ssidCrc8, bssidCrc8, totalLength);

            _framesBuilder.Clear();

            this.DoEncode(totalLength, (byte)passwordBytes.Length,
                 ssidCrc8, bssidCrc8, totalXor, senderIPAddress, passwordBytes, ssid, bssid);
            return _framesBuilder;
        }

        private static byte ComputeTotalXor(
            byte[] senderIPAddress, byte[] passwordBytes, byte[] ssid, byte ssidCrc8, byte bssidCrc8, byte totalLength)
        {
            byte totalXor = 0;
            totalXor ^= (byte)totalLength;
            totalXor ^= (byte)passwordBytes.Length;
            totalXor ^= (byte)ssidCrc8;
            totalXor ^= (byte)bssidCrc8;
            foreach (var b in senderIPAddress)
            {
                totalXor ^= b;
            }
            foreach (var b in passwordBytes)
            {
                totalXor ^= b;
            }
            foreach (var b in ssid)
            {
                totalXor ^= b;
            }

            return totalXor;
        }

        public static IEnumerable<ushort> ByteToFrames(int index, byte b)
        {
            if (index > byte.MaxValue || index < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(index));
            }

            var crc = new Crc8();
            crc.Update(b);
            crc.Update((byte)index);
            var first = (crc.Value & 0xF0) | ((b >> 4) & 0x0F);
            var seq = (0x100 | index);
            var last = ((crc.Value << 4) & 0xF0) | (b & 0x0F);

            yield return (ushort)(first + ExtraLength);
            yield return (ushort)(seq + ExtraLength);
            yield return (ushort)(last + ExtraLength);
        }

        private void AppendByte(byte b)
        {
            this.AppendByte(_framesBuilder.Count / 3, b);
        }

        private void AppendByte(int frameIndex, byte b)
        {
            var fs = ByteToFrames(frameIndex, b);
            _framesBuilder.AddRange(fs);
        }

        private void AppendBytes(IEnumerable<byte> bytes)
        {
            foreach (var b in bytes)
            {
                AppendByte(b);
            }
        }

        private void DoEncode(
            byte totalLength,
            byte passwordLength,
            byte ssidCrc8,
            byte bssidCrc8,
            byte totalXor,
            byte[] senderIpAddress,
            byte[] stationPassword,
            byte[] ssid,
            byte[] bssid)
        {
            this.AppendByte(totalLength);
            this.AppendByte(passwordLength);
            this.AppendByte(ssidCrc8);
            this.AppendByte(bssidCrc8);
            this.AppendByte(totalXor);
            this.AppendBytes(senderIpAddress);
            this.AppendBytes(stationPassword);
            this.AppendBytes(ssid);

            var bssidPos = ExtraHeaderLength;
            for (int i = 0; i < bssid.Length; i++)
            {
                int frameIndex = totalLength + i;
                var byteValue = bssid[i];
                var codeCount = _framesBuilder.Count / 3;
                if (bssidPos >= codeCount)
                {
                    this.AppendByte(byteValue);
                }
                else
                {
                    var fs = ByteToFrames(frameIndex, byteValue);
                    var framesInsertPos = bssidPos * 3;
                    _framesBuilder.InsertRange(framesInsertPos, fs);
                }
                bssidPos += 4;
            }
        }
    }
}
