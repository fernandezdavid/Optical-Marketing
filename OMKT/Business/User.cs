using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace OMKT.Business
{
    public class User
    {
        [Key]
        public virtual Guid UserId { get; set; }

        [Required]
        [DisplayName("Nombre de usuario")]
        public virtual String Username { get; set; }

        [Required]
        [RegularExpression(@"\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*", ErrorMessage = "Debes ingresar un email válido")]
        public virtual String Email { get; set; }

        [Required, DataType(DataType.Password)]
        public virtual String Password { get; set; }

        [DisplayName("Nombre")]
        public virtual String FirstName { get; set; }

        [DisplayName("Apellido")]
        public virtual String LastName { get; set; }

        [DataType(DataType.MultilineText)]
        public virtual String Comment { get; set; }

        [DisplayName("Aprobar")]
        public virtual Boolean IsApproved { get; set; }

        public virtual int PasswordFailuresSinceLastSuccess { get; set; }

        public virtual DateTime? LastPasswordFailureDate { get; set; }

        public virtual DateTime? LastActivityDate { get; set; }

        public virtual DateTime? LastLockoutDate { get; set; }

        public virtual DateTime? DateOfBirth { get; set; }

        [DisplayName("Última visita")]
        public virtual DateTime? LastLoginDate { get; set; }

        public virtual String ConfirmationToken { get; set; }

        [DisplayName("Fecha de creación")]
        public virtual DateTime? CreateDate { get; set; }

        [DisplayName("Bloquear")]
        public virtual Boolean IsLockedOut { get; set; }

        public virtual DateTime? LastPasswordChangedDate { get; set; }

        public virtual String PasswordVerificationToken { get; set; }

        public virtual DateTime? PasswordVerificationTokenExpirationDate { get; set; }

        public virtual ICollection<Role> Roles { get; set; }

        public virtual ICollection<Inbox> Inboxes { get; set; }

        public int CustomerId { get; set; }

        public virtual Customer Customer { get; set; }
    }
}