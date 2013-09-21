using System;

namespace OMKT.Business
{
    public class CatalogDetail
    {
        public int CatalogDetailId { get; set; }

        public int AdvertId { get; set; }

        public virtual Catalog Catalog { get; set; }

        public int Position { get; set; }

        public int CommercialProductId { get; set; }

        public virtual CommercialProduct CommercialProduct { get; set; }

        public double Discount { get; set; }

        public string QRCode { get; set; }

        public string Link { get; set; }

        public string Status { get; set; }

        public DateTime? CreatedDate { get; set; }

        public DateTime? LastUpdate { get; set; }
    }
}