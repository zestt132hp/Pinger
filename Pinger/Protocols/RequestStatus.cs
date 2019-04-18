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

        public RequestStatus(bool isSuccess)
        {
            IsSucces = isSuccess;
        }

        public void SendReqest(IProtocol protocol)
        { 
            /*тут нужно подумать, что делать для пингования,
             1. Нужно вернуть статус с его булевым состоянием*/
        }
    }
}
