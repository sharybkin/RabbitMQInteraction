using System;

namespace MQCommonObjects.Contracts
{
    public class QueueMessageDetail
    {
        public Guid Id { get; }
        public DateTime CreateDate { get; }

        public QueueMessageDetail()
        {
            Id = Guid.NewGuid();
            CreateDate = DateTime.UtcNow;
        }

        public QueueMessageDetail(Guid id, DateTime createDate)
        {
            Id = id;
            CreateDate = createDate;
        }
    }
}