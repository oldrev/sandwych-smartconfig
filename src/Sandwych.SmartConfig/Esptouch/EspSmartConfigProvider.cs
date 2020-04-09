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
            yield return (StandardOptionNames.BroadcastingTargetPort, 7001);
            yield return (StandardOptionNames.ListeningPort, 18266);
            yield return (StandardOptionNames.FrameInterval, TimeSpan.FromMilliseconds(8));
            yield return (StandardOptionNames.SegmentInterval, TimeSpan.FromMilliseconds(8));
            yield return (StandardOptionNames.GuideCodeTimeout, TimeSpan.FromSeconds(2));
            yield return (EspOptionNames.DatumPeriodTimeout, TimeSpan.FromSeconds(4));
        }

        public override IProcedureEncoder CreateProcedureEncoder()
            => new EspProcedureEncoder();

    }
}
