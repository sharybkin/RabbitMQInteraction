using System.Threading.Tasks;

namespace MQReceiver
{
    public interface IReceiver<T>
    {
        public Task Receive();
    }
}