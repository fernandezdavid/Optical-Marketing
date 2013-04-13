using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using OMKT.Business;

namespace OMKT.Models
{
    public class Summary
    {
        public int Year { get; set; }

        public DateTime From { get; set; }
        public DateTime To { get; set; }

        public int Impressions { get; set; }
        public int Traffic { get; set; }

        public List<AdvertCampaign> AdvertCampaigns { get; set; }

    }
}
