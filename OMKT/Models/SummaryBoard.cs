using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OMKT.Models
{
    public class SummaryBoard
    {
        public int Impressions { get; set; }

        public string LikesPercentage { get; set; }

        public string TimeAverage { get; set; }

        public string WinsAverage { get; set; }
    }
}