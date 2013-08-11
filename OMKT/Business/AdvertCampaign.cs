using System;
using System.Collections.Generic;

namespace OMKT.Business
{
    public class AdvertCampaign
    {
        public int AdvertCampaignId { get; set; }

        public int CustomerId { get; set; }

        public virtual Customer Customer { get; set; }

        public string Name { get; set; }

        public int NetworkId { get; set; }

        public virtual Network Network { get; set; }

        public int CampaignLocationId { get; set; }

        public virtual CampaignLocation CampaignLocation { get; set; }

        public DateTime? EndDatetime { get; set; }

        public DateTime? StartDatetime { get; set; }

        public DateTime? LastUpdate { get; set; }

        public DateTime? CreatedDate { get; set; }

        public int CampaignStateId { get; set; }

        public virtual CampaignState CampaignState { get; set; }

        public int CampaignTypeId { get; set; }

        public virtual CampaignType CampaignType { get; set; }

        public decimal? Estimate { get; set; }

        public decimal? Offer { get; set; }

        public ICollection<AdvertCampaignDetail> AdvertCampaignDetails { get; set; }

        public virtual ICollection<AdvertHost> AdvertHosts { get; set; }

        public AdvertCampaign()
        {
            AdvertCampaignDetails = new List<AdvertCampaignDetail>();
        }
    }
}