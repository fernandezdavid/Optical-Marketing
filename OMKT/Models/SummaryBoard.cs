using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OMKT.Models
{
    public class SummaryBoard
    {
        public int Impressions { get; set; }

        public decimal LikesPercentage { get; set; }

        public decimal TimeAverage { get; set; }

        public decimal Bounce { get; set; }
    }
}