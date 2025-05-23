using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PMT
{
    internal struct GemClickEvent : IEventData
    {
        public Gem Gem;

        public GemClickEvent(Gem gem)
        {
            Gem = gem;
        }
    }
}
