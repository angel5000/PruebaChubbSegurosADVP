using AutoMapper;
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
    public class AseguradosApplication : IAseguradosApplication
    {
        private readonly IAseguradosRepository _repo;
        private readonly IMapper _mapper;

        public AseguradosApplication(IAseguradosRepository repo, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }

        async public Task<BaseResponse<AseguradosResponseDTO>> AseguradosporID(int AseguradoID)
        {
            var response = new BaseResponse<AseguradosResponseDTO>();
            try
            {
                // Obtener datos del repositorio (ADO.NET)
                var seguro = await _repo.GetByIdAsync(AseguradoID);

                if (seguro == null)
                {
                    response.IsSucces = false;
                    response.Message = "No se encontró un asegurado con ese ID.";
                    return response;
                }
                var dto = new AseguradosResponseDTO
                {
                  
                    IDASEGURADOS = seguro.IDASEGURADOS,
                    CEDULA = seguro.CEDULA,
                    NMBRCOMPLETO = seguro.NMBRCOMPLETO,
                    TELEFONO = seguro.TELEFONO,
                    EDAD = seguro.EDAD,
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

        async public Task<BaseResponse<bool>> EditarAsegurados(int AseguradoID, AseguradosRequestDTO request)
        {
            var response = new BaseResponse<bool>();
            try
            {
                // DTO → Entidad Domain
                var entidad = _mapper.Map<Asegurados>(request);

                // Llamada al repositorio (SP)
                int result = await _repo.UpdateAsync(AseguradoID, entidad);

                if (result > 0)
                {
                    response.Data = true;
                    response.IsSucces = true;
                    response.Message = "Informacion del cliente modificada correctamente.";
                }
                else
                {
                    response.Data = false;
                    response.IsSucces = false;
                    response.Message = "No se pudo modificar la informacion del cliente.";
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

       async public Task<BaseResponse<bool>> EliminarAsegurados(int AseguradoID)
        {
            var response = new BaseResponse<bool>();
            try
            {
                int result = await _repo.DeleteAsync(AseguradoID);

                if (result > 0)
                {
                    response.Data = true;
                    response.IsSucces = true;
                    response.Message = "Cliente eliminado correctamente.";
                }
                else
                {
                    response.Data = false;
                    response.IsSucces = false;
                    response.Message = "No se pudo eliminar al cliente.";
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

        async  public Task<BaseResponse<IEnumerable<AseguradosResponseDTO>>> ListaAsegurados()
        {
            var response = new BaseResponse<IEnumerable<AseguradosResponseDTO>>();
            try
            {
                // Obtener datos del repositorio (ADO.NET)
                var listaSeguros = await _repo.GetAllAsync();

                // Mapear entidad → DTO
                var listaDto = listaSeguros.Select(x => new AseguradosResponseDTO
                {
                    IDASEGURADOS = x.IDASEGURADOS,
                    CEDULA = x.CEDULA,
                    NMBRCOMPLETO = x.NMBRCOMPLETO,
                    TELEFONO = x.TELEFONO,
                    EDAD = x.EDAD,

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

        async public Task<BaseResponse<bool>> RegistrarAsegurado(AseguradosRequestDTO request)
        {
            var response = new BaseResponse<bool>();
            try
            {
                // DTO → Entidad Domain
                var entidad = _mapper.Map<Asegurados>(request);

                // Llamada al repositorio (SP)
                int result = await _repo.CreateAsync(entidad);

                if (result > 0)
                {
                    response.Data = true;
                    response.IsSucces = true;
                    response.Message = "Cliente registrado correctamente.";
                }
                else
                {
                    response.Data = false;
                    response.IsSucces = false;
                    response.Message = "No se pudo registrar el cliente.";
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
    }
}
