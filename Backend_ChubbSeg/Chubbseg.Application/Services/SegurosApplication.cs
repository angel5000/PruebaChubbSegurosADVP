using AutoMapper;
using Azure.Core;
using Chubbseg.Application.DTOS;
using Chubbseg.Application.Interfaces;
using Chubbseg.Domain.Entidades;
using Chubbseg.Infrastructure.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chubbseg.Application.Services
{
    public class SegurosApplication : ISegurosApplication
    {
        private readonly ISegurosRepository _repo;
        private readonly IMapper _mapper;
        public SegurosApplication(ISegurosRepository repo, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }

       async public Task<BaseResponse<bool>> EditarSeguros(int SeguroID, SegurosRequestDTO request)
        {
            var response = new BaseResponse<bool>();
            try
            {
                // DTO → Entidad Domain
                var entidad = _mapper.Map<Seguros>(request);

                // Llamada al repositorio (SP)
                int result = await _repo.UpdateAsync(SeguroID,entidad);

                if (result > 0)
                {
                    response.Data = true;
                    response.IsSucces = true;
                    response.Message = "Seguro Modificado correctamente.";
                }
                else
                {
                    response.Data = false;
                    response.IsSucces = false;
                    response.Message = "No se pudo modificar el seguro.";
                }
            }
            catch (Exception ex)
            {
                response.Data = false;
                response.IsSucces = false;
                response.Message = $"Error: {ex.Message}";
            }

            return response;
        }

       async public Task<BaseResponse<bool>> EliminarSeguros(int SeguroID)
        {

            var response = new BaseResponse<bool>();
            try
            {
                int result = await _repo.DeleteAsync(SeguroID);

                if (result > 0)
                {
                    response.Data = true;
                    response.IsSucces = true;
                    response.Message = "Seguro eliminado correctamente.";
                }
                else
                {
                    response.Data = false;
                    response.IsSucces = false;
                    response.Message = "No se pudo eliminar el seguro.";
                }
            }
            catch (Exception ex)
            {
                response.Data = false;
                response.IsSucces = false;
                response.Message = $"Error: {ex.Message}";
            }

            return response;
        
        }

        async public Task<BaseResponse<IEnumerable<SegurosResponseDTO>>> ListaSeguros()
        {
            var response = new BaseResponse<IEnumerable<SegurosResponseDTO>>();
            try
            {
                // Obtener datos del repositorio (ADO.NET)
                var listaSeguros = await _repo.GetAllAsync();

                // Mapear entidad → DTO
                var listaDto = listaSeguros.Select(x => new SegurosResponseDTO
                {
                    IDSEGURO=x.IDSEGURO,
                    NMBRSEGURO = x.NMBRSEGURO,
                    CODSEGURO = x.CODSEGURO,
                    SUMASEGURADA = x.SUMASEGURADA,
                    PRIMA = x.PRIMA,
                    EDADMIN = x.EDADMIN,
                    EDADMAX = x.EDADMAX

                });

                response.Data = listaDto;
                response.IsSucces = true;
                response.Message = "Consulta realizada correctamente";
            }
            catch (Exception ex)
            {
                response.IsSucces = false;
                response.Message = ex.Message;
            }

            return response;
        }

       async public Task<BaseResponse<bool>> RegistrarSeguro(SegurosRequestDTO request)
        {
            var response = new BaseResponse<bool>();
            try
            {
                // DTO → Entidad Domain
                var entidad = _mapper.Map<Seguros>(request);

                // Llamada al repositorio (SP)
                int result = await _repo.CreateAsync(entidad);

                if (result > 0)
                {
                    response.Data = true;
                    response.IsSucces = true;
                    response.Message = "Seguro registrado correctamente.";
                }
                else
                {
                    response.Data = false;
                    response.IsSucces = false;
                    response.Message = "No se pudo registrar el seguro.";
                }
            }
            catch (Exception ex)
            {
                response.Data = false;
                response.IsSucces = false;
                response.Message = $"{ex.Message}";
            }

            return response;
        }

       async public Task<BaseResponse<IEnumerable<SegurosResponseDTO>>> SegurosDisponibles(int edad)
        {
            var response = new BaseResponse<IEnumerable<SegurosResponseDTO>>();
            try
            {
                // Obtener datos del repositorio (ADO.NET)
                var listaSeguros = await _repo.GetSelectlistAsync(edad);

                // Mapear entidad → DTO
                var listaDto = listaSeguros.Select(x => new SegurosResponseDTO
                {
                    IDSEGURO = x.IDSEGURO,
                    NMBRSEGURO = x.NMBRSEGURO,
                    CODSEGURO = x.CODSEGURO,
                    SUMASEGURADA = x.SUMASEGURADA,
                    PRIMA = x.PRIMA,
                    EDADMIN = x.EDADMIN,
                    EDADMAX = x.EDADMAX

                });

                response.Data = listaDto;
                response.IsSucces = true;
                response.Message = "Consulta realizada correctamente";
            }
            catch (Exception ex)
            {
                response.IsSucces = false;
                response.Message = ex.Message;
            }

            return response;
        }

        async public Task<BaseResponse<SegurosResponseIDDTO>> SegurosporID(int SeguroID)
        {
            var response = new BaseResponse<SegurosResponseIDDTO>();
            try
            {
                // Obtener datos del repositorio (ADO.NET)
                var seguro = await _repo.GetByIdAsync(SeguroID);

                if (seguro == null)
                {
                    response.IsSucces = false;
                    response.Message = "No se encontró un seguro con ese ID.";
                    return response;
                }
                var dto = new SegurosResponseIDDTO
                {
                    IDSEGURO = seguro.IDSEGURO,
                    NMBRSEGURO = seguro.NMBRSEGURO,
                    CODSEGURO = seguro.CODSEGURO,
                    SUMASEGURADA = seguro.SUMASEGURADA,
                    PRIMA = seguro.PRIMA,
                    EDADMAX =seguro.EDADMAX,
                    EDADMIN = seguro.EDADMIN
                };
                response.Data = dto;
                response.IsSucces = true;
                response.Message = "Consulta realizada correctamente";
            }
            catch (Exception ex)
            {
                response.IsSucces = false;
                response.Message = ex.Message;
            }

            return response;
        }
    }
}
