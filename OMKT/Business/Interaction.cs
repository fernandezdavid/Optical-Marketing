using System;

namespace OMKT.Business
{
    public class Interaction
    {
        public int InteractionId { get; set; }

        public DateTime? StartDateTime { get; set; }

        public DateTime? EndDateTime { get; set; }

        public int Impressions { get; set; }

        //public int Likes { get; set; }

        //public int Views { get; set; }

        public int Traffic { get; set; }

        public int SnapshotId { get; set; }

        public virtual Snapshot Snapshot { get; set; }

        public virtual AdvertCampaignDetail AdvertCampaignDetail { get; set; }

        //public int MonitoringId {get;set;}
        //public virtual Monitoring Monitoring { get; set; }
    }
}