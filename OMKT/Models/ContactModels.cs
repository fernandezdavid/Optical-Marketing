using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace OMKT.Models
{
    public class ContactUsModel
    {
        [Required]
        [Display(Name = "Nombre")]
        public string Name { get; set; }

        [Required]
        [DataType(DataType.EmailAddress)]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required]
        [Display(Name = "Asunto")]
        public string Subject { get; set; }

        [Required]
        [Display(Name = "Teléfono")]
        public string Phone { get; set; }

        [Required]
        [Display(Name = "Mensaje")]
        public string Message { get; set; }
        
        
    }
}