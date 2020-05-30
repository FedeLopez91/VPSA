using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace VPSA.Models
{
    public class DenunciaViewModel
    {
        public int Id { get; set; }
        public DateTime? Fecha { get; set; }
        public string Calle { get; set; }
        public int? Numero { get; set; }
        public string EntreCalles1 { get; set; }
        public string EntreCalles2 { get; set; }
        [Required(ErrorMessage = "*Descripción es Obligatorio")]
        public string Descripcion { get; set; }
        [Required(ErrorMessage = "*Ordenanza infringida es Obligatorio")]
        public int TipoDenunciaId { get; set; }
        public int? EstadoDenunciaId { get; set; }
        public string Legajo { get; set; }
        public string IpDenunciante { get; set; }
        public string NroDenuncia { get; set; }
        [NotMapped]
        public IFormFile Foto { get; set; }
    }
}
