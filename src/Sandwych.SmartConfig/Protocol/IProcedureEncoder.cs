using System.Collections.Generic;

namespace Sandwych.SmartConfig.Protocol
{
    public interface IProcedureEncoder
    {
        IEnumerable<Segment> Encode(SmartConfigContext context, SmartConfigArguments args);
    }
}
