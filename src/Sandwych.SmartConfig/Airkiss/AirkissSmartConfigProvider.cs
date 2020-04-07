using Sandwych.SmartConfig.Airkiss.Protocol;
using Sandwych.SmartConfig.Protocol;
using System;
using System.Collections.Generic;

namespace Sandwych.SmartConfig.Airkiss
{
    public class AirkissSmartConfigProvider : AbstractSmartConfigProvider
    {
        public override string Name => "Airkiss";

        public override IDevicePacketInterpreter CreateDevicePacketInterpreter() => new AirkissDevicePacketInterpreter();

        public override IEnumerable<(string key, object value)> GetDefaultOptions()
        {
            yield return (StandardProperties.BroadcastingTargetPort, 10001); // The port to broadcast doesn't matter
            yield return (StandardProperties.ListeningPort, 10000);
            yield return (StandardProperties.FrameInterval, TimeSpan.FromMilliseconds(0));
            yield return (StandardProperties.SegmentInterval, TimeSpan.FromMilliseconds(5));
            yield return (StandardProperties.GuideCodeTimeout, TimeSpan.FromSeconds(2));

            yield return (AirkissProperties.MagicCodeTimeout, TimeSpan.FromMilliseconds(500));
            yield return (AirkissProperties.PrefixCodeTimeout, TimeSpan.FromMilliseconds(500));

            var randomValue = (byte)(Environment.TickCount % 256);
            yield return (AirkissProperties.RandomNumber, randomValue);
        }

        public override IProcedureEncoder CreateProcedureEncoder()
            => new AirkissProcedureEncoder();

    }
}
