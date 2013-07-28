using System;
using System.Collections.Generic;

namespace OMKT.Business
{
    public class AdvertCampaignInteraction
    {
        public int AdvertCampaignInteractionID { get; set; }

        public int AdvertCampaignID { get; set; }

        public virtual AdvertCampaign AdvertCampaign { get; set; }

        public DateTime? StartDatetime { get; set; }

        public DateTime? EndDatetime { get; set; }

        public ICollection<AdvertCampaignDetailInteraction> AdvertCampaignDetailInteractions { get; set; }

        public AdvertCampaignInteraction()
        {
            AdvertCampaignDetailInteractions = new List<AdvertCampaignDetailInteraction>();
        }
    }
}