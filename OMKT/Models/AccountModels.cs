using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Web.Mvc;
using System.Web.Security;

namespace OMKT.Models
{

    public class ChangePasswordModel
    {
        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Contraseña actual")]
        public string OldPassword { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "Tu contraseña debe tener al menos {2} caracteres.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Nueva contraseña")]
        public string NewPassword { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirmar nueva contraseña")]
        [Compare("NewPassword", ErrorMessage = "Las contraseñas no coinciden.")]
        public string ConfirmPassword { get; set; }
    }

    public class LogOnModel
    {
        [Required]
        [Display(Name = "Usuario")]
        public string User { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Contraseña")]
        public string Password { get; set; }

        [Display(Name = "Recordarme")]
        public bool RememberMe { get; set; }
    }

    public class ProfileModel
    {
        [Display(Name="Nombre")]
        public string Firstname { get; set; }

        [Display(Name = "Apellido")]
        public string Lastname { get; set; }
        
        [DataType(DataType.DateTime)]
        [Display(Name = "Fecha de nacimiento")]
        public DateTime? DateOfBirth { get; set; }

        [Required]
        [Display(Name = "Email")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [Required]
        [Display(Name = "Compañía")]
        public string CompanyName { get; set; }

        [Required]
        [Display(Name = "Dirección")]
        public string CompanyAdress { get; set; }

        [Required]
        [Display(Name = "Código Postal")]
        public string CompanyCP { get; set; }

        [Required]
        [Display(Name = "Ciudad")]
        public string CompanyCity { get; set; }

        [Required]
        [Display(Name = "Contacto")]
        public string ContactPerson { get; set; }

        [Required]
        [Display(Name = "Teléfono")]
        public string CompanyPhone { get; set; }

    }

    public class RegisterModel
    {
        [Required]
        [Display(Name = "Nombre")]
        public string Name { get; set; }

        [Required]
        [Display(Name = "Apellido")]
        public string Lastname { get; set; }

        [Required]
        [DataType(DataType.EmailAddress)]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.DateTime)]
        [Display(Name = "Fecha de nacimiento")]
        public string DateOfBirth  { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "Tu contraseña debe contener al menos {2} caracteres.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Contraseña")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirmar contraseña")]
        [Compare("Password", ErrorMessage = "Las contraseñas no coinciden.")]
        public string ConfirmPassword { get; set; }
    }
}
