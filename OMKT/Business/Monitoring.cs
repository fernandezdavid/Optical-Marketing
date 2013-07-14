using System;
using System.Collections.Generic;

namespace OMKT.Business
{
    public class Monitoring
    {
        public int MonitoringID { get; set; }

        public int AdvertHostID {get;set;}
        public virtual AdvertHost AdvertHost { get; set; }

        public DateTime? Timestamp { get; set; }

        public float Average { get; set; }

    }
}