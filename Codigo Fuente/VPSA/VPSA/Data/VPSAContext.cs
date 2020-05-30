using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VPSA.Models;

namespace VPSA.Data
{
    public class VPSAContext : DbContext
    {
        public VPSAContext(DbContextOptions<VPSAContext> options)
            : base(options)
        {
        }

        public DbSet<Denuncia> Denuncias { get; set; }
        public DbSet<TipoDenuncia> TiposDenuncia { get; set; }
        public DbSet<EstadoDenuncia> EstadosDenuncia { get; set; }
        public DbSet<VPSA.Models.DenunciaViewModel> DenunciaViewModel { get; set; }
    }
}
