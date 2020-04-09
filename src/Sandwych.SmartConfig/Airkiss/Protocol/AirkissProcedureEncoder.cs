using Sandwych.SmartConfig.Protocol;
using Sandwych.SmartConfig.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sandwych.SmartConfig.Airkiss.Protocol
{
    public struct AirkissProcedureEncoder : IProcedureEncoder
    {
        public IEnumerable<Segment> Encode(SmartConfigContext context, SmartConfigArguments args)
        {
            var frameInterval = context.GetOption<TimeSpan>(StandardOptionNames.FrameInterval);

            var builder = new List<Segment>(128);

            var ssid = Encoding.UTF8.GetBytes(args.Ssid);
            var ssidCrc8 = Crc8.ComputeOnceOnly(ssid);
            var password = args.Password != null ? Encoding.UTF8.GetBytes(args.Password) : Constants.EmptyBuffer;

            // Guide Segment
            var guidePeriod = context.GetOption<TimeSpan>(StandardOptionNames.GuideCodeTimeout);
            builder.Add(new Segment(AirkissWellknownConstants.GuideCodes, frameInterval, guidePeriod));

            // Magic Code Segment
            var magicCodeFrames = AirkissMagicCodeFrameEncoder.Encode(password.Length + ssid.Length + 1, ssidCrc8);
            var magicCodeTimeout = context.GetOption<TimeSpan>(AirkissOptionNames.MagicCodeTimeout);
            builder.Add(new Segment(magicCodeFrames, frameInterval, magicCodeTimeout));

            // Prefix Code Segment
            var prefixCodeFrames = AirkissPrefixCodeFrameEncoder.Encode(password.Length);
            var prefixCodeTimeout = context.GetOption<TimeSpan>(AirkissOptionNames.PrefixCodeTimeout);
            builder.Add(new Segment(prefixCodeFrames, frameInterval, prefixCodeTimeout));

            // Data(password/random/ssid) Segment
            var randValue = context.GetOption<byte>(AirkissOptionNames.RandomNumber);
            var buf = password.Append<byte>(randValue).Concat(ssid).ToList();
            var dataFrames = new List<ushort>(buf.Count * 2);
            var index = 0;
            foreach (var bytes in buf.Partition(4))
            {
                var seqEntryFrames = AirkissSeqEntryFrameEncoder.Encode(index, bytes);
                dataFrames.AddRange(seqEntryFrames);
                index++;
            }
            builder.Add(new Segment(dataFrames, frameInterval, TimeSpan.FromSeconds(4)));

            return builder;
        }
    }
}
