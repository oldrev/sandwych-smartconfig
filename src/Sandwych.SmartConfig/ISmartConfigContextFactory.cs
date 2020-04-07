using System;
using System.Collections.Generic;
using System.Text;

namespace Sandwych.SmartConfig
{
    public interface ISmartConfigContextFactory
    {
        SmartConfigContext CreateContext();
    }
}
