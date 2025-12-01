using AutoMapper;
using Chubbseg.Application.DTOS;
using Chubbseg.Domain.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chubbseg.Application.Mappers
{
    public class SegurosMapping:Profile
    {
        public SegurosMapping()
        {

            CreateMap<Seguros, SegurosResponseDTO>();
            CreateMap<SegurosResponseDTO, Seguros>();
            CreateMap<Asegurados, AseguradosResponseDTO>();
            CreateMap<AseguradosResponseDTO, Asegurados>();
            CreateMap<SegurosRequestDTO, Seguros>();
            CreateMap<AseguradosRequestDTO, Asegurados>();
            CreateMap<SegurosResponseIDDTO, Asegurados>();
            CreateMap<Aseguramiento,AseguramientoResponseDTO>();
            CreateMap<AseguramientoRequestDTO, Aseguramiento>();
        }
    }
}
