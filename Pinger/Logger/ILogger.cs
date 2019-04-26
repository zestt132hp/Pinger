
namespace Pinger.Logger
{
    public interface ILogger
    {
        void Write<T>(T message);
    }
}
