using System.Threading.Tasks;
using MQCommonObjects.Contracts;

namespace MQSender
{
    public interface IOrderSender
    {
        public Task Send(Order order);
    }
}