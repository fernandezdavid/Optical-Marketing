using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace OMKT.Business
{
    public sealed class Invoice
    {
        public Invoice()
        {
            InvoiceDetails = new List<InvoiceDetails>();
        }

        public int InvoiceID { get; set; }

        [DisplayName("Número de Factura")]
        public int InvoiceNumber { get; set; }

        public bool IsProposal
        {
            get
            {
                return (InvoiceNumber == 0);
            }
        }

        public int CustomerID { get; set; }

        public Customer Customer { get; set; }

        public string Name { get; set; }

        [DisplayName("Nombre/Notas")]
        [Required]
        public string Notes { get; set; }

        [DisplayName("Detalle de propuesta")]
        public string ProposalDetails { get; set; }

        [DisplayName("Creada")]
        public DateTime TimeStamp { get; set; }

        [DisplayName("Fecha de vencimiento")]
        public DateTime DueDate { get; set; }

        [DisplayName("Impuesto")]
        [Range(0.00, 100.0, ErrorMessage = "Value must be a % between 0 and 100")]
        public decimal AdvancePaymentTax { get; set; }

        public bool Paid { get; set; }

        public ICollection<InvoiceDetails> InvoiceDetails { get; set; }

        #region Calculated fields

        public decimal VATAmount
        {
            get
            {
                return TotalWithVAT - NetTotal;
            }
        }

        /// <summary>
        /// Total before TAX
        /// </summary>
        public decimal NetTotal
        {
            get
            {
                if (InvoiceDetails == null)
                    return 0;

                return InvoiceDetails.Sum(i => i.Total);
            }
        }

        public decimal AdvancePaymentTaxAmount
        {
            get
            {
                if (InvoiceDetails == null)
                    return 0;

                return NetTotal * (AdvancePaymentTax / 100);
            }
        }

        /// <summary>
        /// Total with tax
        /// </summary>
        public decimal TotalWithVAT
        {
            get
            {
                if (InvoiceDetails == null)
                    return 0;

                return InvoiceDetails.Sum(i => i.TotalPlusVAT);
            }
        }

        /// <summary>
        /// Total with VAT minus advanced tax payment
        /// </summary>
        public decimal TotalToPay
        {
            get
            {
                return TotalWithVAT - AdvancePaymentTaxAmount;
            }
        }

        #endregion Calculated fields
    }
}