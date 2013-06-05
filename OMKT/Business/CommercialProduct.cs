using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace OMKT.Business
{
    public class CommercialProduct
    {
        public int CommercialProductId { get; set; }

        public string Description { get; set; }

        public string ProductName { get; set; }

        public virtual ProductImage ProductImage { get; set; }
        public int ProductImageId { get; set; }

        public virtual CommercialProductType CommercialProductType { get; set; }
        public int CommercialProductTypeId { get; set; }

        public int Stock { get; set; }

        public decimal? Price { get; set; }

        public string VideoPath { get; set; }

        public int CustomerId { get; set; }
        public virtual Customer Customer { get; set; }
    }
}