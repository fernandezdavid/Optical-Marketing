using System;

namespace OMKT.Business
{
    public class Inbox
    {
        public int InboxId { get; set; }

        public string Message { get; set; }

        public int FromId { get; set; }

        public virtual User To { get; set; }

        public int ToId { get; set; }

        public DateTime Timestamp { get; set; }

        public Boolean Unread { get; set; }
    }
}