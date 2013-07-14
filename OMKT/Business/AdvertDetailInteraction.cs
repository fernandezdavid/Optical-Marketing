using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OMKT.Business
{
    public class AdvertDetailInteraction
    {
        public int AdvertDetailInteractionID { get; set; }

        public int AdvertDetailID { get; set; }
        public virtual AdvertDetail AdvertDetail { get; set; }

        public Boolean View { get; set; }

        public Boolean Like { get; set; }
    }
}