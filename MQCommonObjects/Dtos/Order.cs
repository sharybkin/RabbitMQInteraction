namespace MQCommonObjects.Dtos
{
    public class Order : QueueMessageDetail
    {
        public int OrderId { get; }
        public string OrderDetail { get; }
        
        public Order(int orderId, string orderDetail)
        {
            OrderId = orderId;
            OrderDetail = orderDetail;
        }
    }
}