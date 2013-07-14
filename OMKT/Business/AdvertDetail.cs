using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace OMKT.Business
{
    public class AdvertDetail
    {
        public int AdvertDetailId { get; set; }

        public int AdvertId { get; set; }
        public virtual Catalog Catalog { get; set; }

        public int Position { get; set; }

        public int CommercialProductId { get; set; }
        public virtual CommercialProduct CommercialProduct { get; set; }

        public int Likes { get; set; }

        public int Views { get; set; }

        public string Link { get; set; }

        public DateTime? CreatedDate { get; set; }

        public DateTime? LastUpdate { get; set; }
    }
}