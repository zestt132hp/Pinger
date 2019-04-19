using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;

namespace Pinger.Protocols
{
    class RequestStatus
    {
        public bool IsSucces { get; private set; }
        public String Message { get; set; }

        public RequestStatus(bool isSuccess)
        {
            IsSucces = isSuccess;
        }
    }
}
