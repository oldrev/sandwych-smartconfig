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
            yield return (StandardOptionNames.BroadcastingTargetPort, 10001); // The port to broadcast doesn't matter
            yield return (StandardOptionNames.ListeningPort, 10000);
            yield return (StandardOptionNames.FrameInterval, TimeSpan.Zero);
            yield return (StandardOptionNames.SegmentInterval, TimeSpan.FromMilliseconds(5));
            yield return (StandardOptionNames.GuideCodeTimeout, TimeSpan.FromSeconds(2));

            yield return (AirkissOptionNames.MagicCodeTimeout, TimeSpan.FromMilliseconds(500));
            yield return (AirkissOptionNames.PrefixCodeTimeout, TimeSpan.FromMilliseconds(500));

            var randomValue = (byte)(Environment.TickCount % 256);
            yield return (AirkissOptionNames.RandomNumber, randomValue);
        }

        public override IProcedureEncoder CreateProcedureEncoder()
            => new AirkissProcedureEncoder();

    }
}
