using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OMKT.Business
{
    public class GameDetail
    {
        public int GameDetailId { get; set; }

        public int AdvertId { get; set; }

        public virtual Game Game { get; set; }

        public int CommercialProductId { get; set; }

        public virtual CommercialProduct CommercialProduct { get; set; }

        public double Discount { get; set; }
        
        public string QRCode { get; set; }

        public DateTime? CreatedDate { get; set; }

        public DateTime? LastUpdate { get; set; }
    }
}