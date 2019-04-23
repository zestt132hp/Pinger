
namespace Pinger.Logger
{
    interface ILogger
    {
        void Write<T>(T message);
    }
}
