using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OMKT.Business
{
    public class AdvertCampaignInteraction
    {
        public int AdvertCampaignInteractionID { get; set; }

        public int AdvertCampaignID { get; set; }
        public virtual AdvertCampaign AdvertCampaign { get; set; }

        public DateTime? StartDatetime { get; set; }

        public DateTime? EndDatetime { get; set; }

        public ICollection<AdvertInteraction> AdvertInteractions { get; set; }

        public AdvertCampaignInteraction()
        {
            AdvertInteractions = new List<AdvertInteraction>();
        }
    }
}