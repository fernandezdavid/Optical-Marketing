using System;

namespace OMKT.Business
{
    public class CatalogDetailInteraction
    {
        public int CatalogDetailInteractionID { get; set; }

        public int CatalogDetailID { get; set; }

        public virtual CatalogDetail CatalogDetail { get; set; }

        public Boolean View { get; set; }

        public Boolean Like { get; set; }
    }
}