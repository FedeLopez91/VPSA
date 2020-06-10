
using AutoMapper;
using VPSA.Models;

namespace VPSA.Helpers
{
    public class AutoMapping: Profile
    {
        public AutoMapping()
        {
            CreateMap<DenunciaViewModel, Denuncia>()
                .ForMember(c => c.TipoDenuncia, option => option.Ignore())
                .ForMember(c=> c.EstadoDenuncia, option => option.Ignore());
        }
    }
}
