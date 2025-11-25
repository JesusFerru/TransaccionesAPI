using AutoMapper;
using Transacciones.API.Endpoints.Cuentas;
using Transacciones.API.Endpoints.Transacciones;
using Transacciones.Core.Entities.CuentaAggregate;
using Transacciones.Core.Entities.TransaccionAggregate;

namespace Transacciones.API.Mapper
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<CreateCuentaRequest, Cuenta>();
            CreateMap<Cuenta, CreateCuentaResponse>();
            CreateMap<Cuenta, CuentaResponse>();
            CreateMap<Transaccion, TransaccionResponse>();
        }
    }
}
