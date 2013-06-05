using System;
using System.ComponentModel.DataAnnotations;

namespace OMKT.Business
{
    public class Email
    {
        public int EmailId { get; set; }

        public string FromName { get; set; }

        public string From { get; set; }

        public string To { get; set; }

        public string Subject { get; set; }

        public DateTime Date { get; set; }

        public string Message { get; set; }

        public User User { get; set; }
    }
}