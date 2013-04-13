using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace OMKT.Business
{
    public class AdvertDetail
    {
        public int AdvertDetailId { get; set; }

        [Required]
        public int AdvertId { get; set; }

        public virtual Catalog Catalog { get; set; }

        [DisplayName("Nro. de orden")]
        public int Position { get; set; }

        public int CommercialProductId { get; set; }

        [DisplayName("Producto")]
        public virtual CommercialProduct CommercialProduct { get; set; }

        [Required]
        public int Likes { get; set; }

        [Required]
        public int Views { get; set; }

        [DisplayName("Creado")]
        public DateTime CreatedDate { get; set; }

        public DateTime? LastUpdate { get; set; }
    }
}