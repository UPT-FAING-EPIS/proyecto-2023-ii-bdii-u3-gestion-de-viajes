using AutoMapper;
using ClienteAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ClienteAPI.DTOs;

namespace ClienteAPI{
    public class MappingProfile : Profile{
        public MappingProfile(){
            /* CreateMap<ClienteCreateDTO, Cliente>(); */
            CreateMap<ClienteCreateDTO, Cliente>()
                .ForMember(d => d.NomCliente, o => o.MapFrom( s => s.Nombre))
                .ForMember(d => d.ApePaterno, o => o.MapFrom( s => s.ApellidoPaterno))
                .ForMember(d => d.ApeMaterno, o => o.MapFrom( s => s.ApellidoMaterno))
                .ForMember(d => d.Numero, o => o.MapFrom(s =>s.Celular))
                ;
            CreateMap<TipoCorreoDTO, TipoCorreo>();
            CreateMap<TipoDireccionDTO, TipoDireccion>();
            CreateMap<TipoResidenciumDTO, TipoResidencium>();
            CreateMap<TiposDocumentoDTO, TiposDocumento>();
            CreateMap<TipoSeguroDTO, TipoSeguro>();

        }
    }
}
