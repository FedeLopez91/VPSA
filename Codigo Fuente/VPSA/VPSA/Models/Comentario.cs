using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace VPSA.Models
{
    public class Comentario
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "*Descripción es Obligatorio")]
        public string Descripcion { get; set; }
        [Required]
        public int DenunciaId { get; set; }
        public virtual Denuncia Denuncia { get; set; }
        [Required(ErrorMessage = "*Empleado Asignado Obligatorio")]
        public int EmpleadoId { get; set; }
        public virtual Empleado Empleado { get; set; }
        [Required(ErrorMessage = "*Estado es Obligatorio")]
        public int EstadoDenunciaId { get; set; }
        public virtual EstadoDenuncia EstadoDenuncia { get; set; }
        public DateTime FechaCreacion { get; set; }
    }
}
