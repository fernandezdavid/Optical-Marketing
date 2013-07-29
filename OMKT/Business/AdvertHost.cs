using System;

namespace OMKT.Business
{
    public class AdvertHost
    {
        public int AdvertHostId { get; set; }

        public string AdvertHostName { get; set; }

        public int AdvertHostCategoryId { get; set; }

        public virtual AdvertHostCategory AdvertHostCategory { get; set; }

        public DateTime? StartTime { get; set; }

        public DateTime? EndTime { get; set; }

        public DateTime? LastDownTime { get; set; }
        
        public DateTime? LastUpTime{ get; set; }

        public DateTime? UpTime { get; set; }

        public int LocationId { get; set; }

        public virtual Location Location { get; set; }

        public string KinectID { get; set; }

        public Boolean isUp { get; set; }

        //public virtual ICollection<AdvertCampaign> AdvertCampaigns { get; set; }
    }
}