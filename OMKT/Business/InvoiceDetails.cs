using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace OMKT.Business
{
    public class InvoiceDetails
    {
        public int InvoiceDetailsID { get; set; }

        public int InvoiceID { get; set; }

        public virtual Invoice Invoice { get; set; }

        [Required]
        public string Article { get; set; }

        [Range(-100000, 100000, ErrorMessage = "Quantity must be between 1 and 100000")]
        public int Qty { get; set; }

        [Range(0.01, 999999999, ErrorMessage = "Price must be between 0.01 and 999999999")]
        public decimal Price { get; set; }

        [Range(0.00, 100, ErrorMessage = "VAT must be a % between 0 and 100")]
        public decimal VAT { get; set; }

        [DisplayName("Creado")]
        public DateTime TimeStamp { get; set; }

        //public int AdvertCampaignId { get; set; }
        //public virtual AdvertCampaign AdvertCampaign { get; set; }

        #region Calculated fields

        public decimal Total
        {
            get
            {
                return Qty * Price;
            }
        }

        public decimal VATAmount
        {
            get
            {
                return TotalPlusVAT - Total;
            }
        }

        public decimal TotalPlusVAT
        {
            get
            {
                return Total * (1 + VAT / 100);
            }
        }

        #endregion Calculated fields
    }
}