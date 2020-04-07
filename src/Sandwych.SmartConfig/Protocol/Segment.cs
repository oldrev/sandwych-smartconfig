using System;
using System.Collections.Generic;

namespace Sandwych.SmartConfig.Protocol
{
    public struct Segment
    {
        public IEnumerable<ushort> Frames { get; }
        public TimeSpan FrameInterval { get; }
        public TimeSpan BroadcastingPeriod { get; }
        public int BroadcastingMaxTimes { get; }

        public Segment(
            IEnumerable<ushort> frames,
            TimeSpan frameInterval,
            int broadcastingMaxTimes)
        {
            this.Frames = frames;
            this.FrameInterval = frameInterval;
            this.BroadcastingPeriod = TimeSpan.MaxValue;
            this.BroadcastingMaxTimes = broadcastingMaxTimes;
        }

        public Segment(
            IEnumerable<ushort> frames,
            TimeSpan frameInterval,
            TimeSpan broadcastingPeriod)
        {
            this.Frames = frames;
            this.FrameInterval = frameInterval;
            this.BroadcastingPeriod = broadcastingPeriod;
            this.BroadcastingMaxTimes = 0;
        }
    }
}
