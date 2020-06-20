using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace VPSA.Models
{
    public class User : IdentityUser
    {
        public int Legajo { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }

        public virtual string NombreCompleto
        {
            get
            {
                return $"{FirstName} {LastName}";
            }
        }
    }
}
