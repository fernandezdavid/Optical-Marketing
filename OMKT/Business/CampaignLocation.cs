using System.Collections.Generic;

namespace OMKT.Business
{
    public class CampaignLocation
    {
        public int CampaignLocationId { get; set; }

        public string Description { get; set; }

        public ICollection<AdvertHost> AdvertHosts { get; set; }

        public CampaignLocation()
        {
            AdvertHosts = new List<AdvertHost>();
        }
    }
}