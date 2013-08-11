using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace OMKT.Business
{
    public class Customer
    {
        public int CustomerID { get; set; }

        public string Name { get; set; }

        public string CompanyNumber { get; set; }

        public string Address { get; set; }

        public string CP { get; set; }

        public string City { get; set; }

        public string ContactPerson { get; set; }

        public string Phone1 { get; set; }

        public string Phone2 { get; set; }

        public Boolean IsActive { get; set; }

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