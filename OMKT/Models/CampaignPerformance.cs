using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OMKT.Models
{
    public class CampaignPerformance
    {
        public int Traffic { get; set; } //personas que interactuaron

        public int Impressions { get; set; } //veces que se reprodujo

        public decimal Conversions { get; set; } // valoracion en % Conversion?

        public decimal TimeAverage { get; set; } // tiempo promedio de visita

        public decimal HeightAverage { get; set; }

        public decimal Bounce { get; set; } //porcentaje de rebote
        
        public string CampaignName { get; set; } //nombre campanya

        public int Month { get; set; }

    }
}