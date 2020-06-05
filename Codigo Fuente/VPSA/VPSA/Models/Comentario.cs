using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace VPSA.Models
{
    public class Comentario
    {
        [Key]
        public int Id { get; set; }
        public String trabajo { get; set; }
        [Required]
        public int DenunciaId { get; set; }
        public virtual Denuncia Denuncia { get; set; }
        public int EmpleadoId { get; set; }
        public virtual Empleado Empleado { get; set; }

    }
}
