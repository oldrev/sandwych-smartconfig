using Sandwych.SmartConfig.Esptouch.Protocol;
using Sandwych.SmartConfig.Protocol;
using System;
using System.Collections.Generic;

namespace Sandwych.SmartConfig.Esptouch
{
    public class EspSmartConfigProvider : AbstractSmartConfigProvider
    {
        public override string Name => "Esptouch";

        public override IDevicePacketInterpreter CreateDevicePacketInterpreter()
            => new EspDevicePacketInterpreter();

        public override IEnumerable<(string key, object value)> GetDefaultOptions()
        {
            yield return (StandardProperties.BroadcastingTargetPort, 7001);
            yield return (StandardProperties.ListeningPort, 18266);
            yield return (StandardProperties.FrameInterval, TimeSpan.FromMilliseconds(8));
            yield return (StandardProperties.SegmentInterval, TimeSpan.FromMilliseconds(8));
            yield return (StandardProperties.GuideCodeTimeout, TimeSpan.FromSeconds(2));
            yield return (EspProperties.DatumPeriodTimeout, TimeSpan.FromSeconds(4));
        }

        public override IProcedureEncoder CreateProcedureEncoder()
            => new EspProcedureEncoder();

    }
}
