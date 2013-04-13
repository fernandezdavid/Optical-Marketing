using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace OMKT.Business
{
    public class Customer
    {
        public int CustomerID { get; set; }

        [Required]
        [DisplayName("Nombre")]
        public string Name { get; set; }

        [DisplayName("CUIT")]
        [Required]
        public string CompanyNumber { get; set; }

        [Required]
        [DisplayName("Dirección")]
        public string Address { get; set; }

        [Required]
        [DisplayName("CP")]
        public string CP { get; set; }

        [Required]
        [DisplayName("Ciudad")]
        public string City { get; set; }

        [Required]
        [DisplayName("Contacto")]
        public string ContactPerson { get; set; }

        [Required]
        [DisplayName("Teléfono")]
        public string Phone1 { get; set; }

        [DisplayName("Móvil")]
        public string Phone2 { get; set; }

        [DisplayName("Activo")]
        public Boolean IsActive { get; set; }

        [Required]
        [RegularExpression(@"\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*", ErrorMessage = "Debes ingresar un email váilido.")]
        public string Email { get; set; }

        public ICollection<AdvertCampaign> AdvertCampaigns { get; set; }

        public ICollection<User> Users { get; set; }

        public ICollection<CommercialProduct> CommercialProducts { get; set; }

        public Customer()
        {
            CommercialProducts = new List<CommercialProduct>();
        }
    }
}