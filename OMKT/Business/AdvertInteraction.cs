using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OMKT.Business
{
    public class AdvertInteraction
    {
        public int AdvertInteractionID { get; set; }

        public int AdvertID { get; set; }
        public virtual Advert Advert { get; set; }

        public double Height { get; set; }

        public DateTime? StartDatetime { get; set; }

        public DateTime? EndDatetime { get; set; }

        public ICollection<AdvertDetailInteraction> AdvertDetailInteractions { get; set; }

        public AdvertInteraction()
        {
            AdvertDetailInteractions = new List<AdvertDetailInteraction>();
        }

     }
}