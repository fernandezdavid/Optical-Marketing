using System;

namespace OMKT.Business
{
    public class CatalogDetailInteraction
    {
        public int CatalogDetailInteractionID { get; set; }

        public int CatalogDetailID { get; set; }

        public virtual CatalogDetail CatalogDetail { get; set; }

        public Boolean View { get; set; }

        public Boolean? Like { get; set; }

        public DateTime StartDatetime { get; set; }

        public DateTime? EndDatetime { get; set; }

        public int? TimeElapsed { get; set; }


    }
}