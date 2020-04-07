using Sandwych.SmartConfig.Protocol;
using System;
using System.Collections.Generic;

namespace Sandwych.SmartConfig.Esptouch.Protocol
{
    public sealed class EspProcedureEncoder : IProcedureEncoder
    {
        public IEnumerable<Segment> Encode(SmartConfigContext context, SmartConfigArguments args)
        {
            var guideTimeout = context.GetOption<TimeSpan>(StandardProperties.GuideCodeTimeout);
            var datumTimeout = context.GetOption<TimeSpan>(EspProperties.DatumPeriodTimeout);
            var frameInterval = context.GetOption<TimeSpan>(StandardProperties.FrameInterval);

            var datumEncoder = new EspDatumFrameEncoder();
            var segFrames = new Segment[]
            {
                new Segment(EspWellKnownConstants.GuideCodes, frameInterval, guideTimeout),
                new Segment(datumEncoder.Encode(context, args), frameInterval, datumTimeout)
            };
            return segFrames;
        }
    }
}
