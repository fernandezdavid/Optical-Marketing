using System;
using System.ComponentModel.DataAnnotations;

namespace OMKT.Business
{
    public class Email
    {
        public int EmailId { get; set; }

        [Required]
        public string FromName { get; set; }

        [Required]
        public string From { get; set; }

        [Required]
        public string To { get; set; }

        public string Subject { get; set; }

        public DateTime Date { get; set; }

        [Required]
        public string Message { get; set; }

        public User User { get; set; }
    }
}