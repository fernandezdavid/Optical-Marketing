using System;

namespace OMKT.Business
{
    public class Alert
    {
        public int AlertId { get; set; }

        public string Message { get; set; }

        public DateTime? Date { get; set; }

        public virtual Customer Customer { get; set; }
        public int CustomerId { get; set; }
    }
}