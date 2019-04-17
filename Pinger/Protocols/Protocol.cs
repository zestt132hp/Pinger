using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Pinger.Protocols;

namespace Pinger.Modules
{
    abstract class Protocol
    {
        private static Int32 interaval = 5;
        public virtual string Host { get; set; }
        public virtual int Interval { get; set; } = interaval;
    }
}
