
using System;

namespace Pinger.Protocols
{
    public class RequestStatus
    {
        public Boolean IsSucces { get;}

        public RequestStatus(Boolean isSuccess)
        {
            IsSucces = isSuccess;
        }
    }
}
