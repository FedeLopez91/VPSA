using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace VPSA.Models
{
    public class UserLoginModel
    {
        [EmailAddress]
        public string Email { get; set; }
        [Range(1, int.MaxValue, ErrorMessage = "El legajo es requerido")]
        [Required(ErrorMessage = "El legajo es requerido")]
        public int? Legajo { get; set; }
        [Required(ErrorMessage = "La Contraseña es requerida")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        [Display(Name = "Remember me?")]
        public bool RememberMe { get; set; }
    }
}
