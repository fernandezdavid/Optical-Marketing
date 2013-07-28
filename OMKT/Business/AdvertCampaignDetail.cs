using System;
using System.Collections.Generic;

namespace OMKT.Business
{
    public class AdvertCampaignDetail
    {
        public int AdvertCampaignDetailId { get; set; }

        public DateTime? EndDate { get; set; }

        public DateTime? StartDate { get; set; }

        public string AdvertGroup { get; set; }

        public int AdvertCampaignId { get; set; }

        public virtual AdvertCampaign AdvertCampaign { get; set; }

        public int AdvertID { get; set; }

        public virtual Advert Advert { get; set; }

        public int Impressions { get; set; }

        public ICollection<Interaction> Interactions { get; set; }

        public AdvertCampaignDetail()
        {
            Interactions = new List<Interaction>();
        }
    }
}