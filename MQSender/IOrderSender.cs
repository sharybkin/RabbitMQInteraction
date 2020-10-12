using MQCommonObjects.Dtos;

namespace MQSender
{
    public interface IOrderSender
    {
        public void Send(Order order);
    }
}