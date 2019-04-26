
namespace Pinger.Protocols
{
    public class RequestStatus
    {
        public bool IsSucces { get; private set; }

        public RequestStatus(bool isSuccess)
        {
            IsSucces = isSuccess;
        }
    }
}
