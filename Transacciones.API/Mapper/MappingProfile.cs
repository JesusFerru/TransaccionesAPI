using AutoMapper;
using Transacciones.API.Endpoints.Cuentas;
using Transacciones.Core.Entities.CuentaAggregate;

namespace Transacciones.API.Mapper
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<CreateCuentaRequest, Cuenta>();
            CreateMap<Cuenta, CreateCuentaResponse>();
            CreateMap<Cuenta, CuentaResponse>();
        }
    }
}
