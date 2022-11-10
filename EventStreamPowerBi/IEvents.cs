using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventStreamPowerBi
{
    public interface IEvents
    {
        void ProduceChargeEvent();
    }
}
