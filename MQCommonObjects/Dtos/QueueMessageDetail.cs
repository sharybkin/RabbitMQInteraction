using System;

namespace MQCommonObjects.Dtos
{
    public class QueueMessageDetail
    {
        public Guid MessageId { get; }
        public DateTime CreateDate { get; }

        public QueueMessageDetail()
        {
            MessageId = Guid.NewGuid();
            CreateDate = DateTime.UtcNow;
        }

        public QueueMessageDetail(Guid messageId, DateTime createDate)
        {
            MessageId = messageId;
            CreateDate = createDate;
        }
    }
}