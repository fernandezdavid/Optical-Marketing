using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OMKT.Business
{
    public class Game : Advert
    {
        public string QRCode { get; set; }

        public int CommercialProductId { get; set; }
        public virtual CommercialProduct CommercialProduct { get; set; }

        public int Cards { get; set; }

        public double Discount { get; set; }

    }
}