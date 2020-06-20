using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace VPSA.Models
{
    public class UserRegistrationModel
    {
        public int Legajo { get; set; }
        [Required(ErrorMessage = "El nombre es requerido")]
        public string FirstName { get; set; }
        [Required(ErrorMessage = "El apellido es requerido")]
        public string LastName { get; set; }
        [Required(ErrorMessage = "El Email es requerido")]
        [EmailAddress]
        public string Email { get; set; }

        [Required(ErrorMessage = "La Contraseña es requerida")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "La contraseña y la contraseña de confirmación no coinciden.")]
        public string ConfirmPassword { get; set; }
    }
}
