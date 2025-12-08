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
    public class AseguradosApplication : IAseguradosApplication
    {
        private readonly IAseguradosRepository _repo;
        private readonly IMapper _mapper;

        public AseguradosApplication(IAseguradosRepository repo, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }

        public async Task<BaseResponse<AseguradosResponseDTO>> AseguradosporID(int AseguradoID)
        {
            BaseResponse<AseguradosResponseDTO> response = new BaseResponse<AseguradosResponseDTO>();
            try
            {
                Asegurados seguro = await _repo.GetByIdAsync(AseguradoID);

                if (seguro == null)
                {
                    response.IsSucces = false;
                    response.Message = "No se encontró un asegurado con ese ID.";
                    return response;
                }
                AseguradosResponseDTO dto = _mapper.Map<AseguradosResponseDTO>(seguro);

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

        public async Task<BaseResponse<bool>> EditarAsegurados(int AseguradoID, AseguradosEditRequestDTO request)
        {
            BaseResponse<bool> response = new BaseResponse<bool>();
            try
            {

                Asegurados entidad = _mapper.Map<Asegurados>(request);
                int result = await _repo.UpdateAsync(AseguradoID, entidad);

                if (result > 0)
                {
                    response.Data = true;
                    response.IsSucces = true;
                    response.Message = "Información del cliente modificada correctamente.";
                }
                else
                {
                    response.Data = false;
                    response.IsSucces = false;
                    response.Message = "No se pudo modificar la información del cliente.";
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

        public async Task<BaseResponse<bool>> EliminarAsegurados(int AseguradoID)
        {
            BaseResponse<bool> response = new BaseResponse<bool>();
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

        public async Task<BaseResponse<IEnumerable<AseguradosResponseDTO>>> ListaAsegurados()
        {
            BaseResponse<IEnumerable<AseguradosResponseDTO>> response = new BaseResponse<IEnumerable<AseguradosResponseDTO>>();
            try
            {
                List<Asegurados> listaSeguros = await _repo.GetAllAsync();
                IEnumerable<AseguradosResponseDTO> listaDto = _mapper.Map<IEnumerable<AseguradosResponseDTO>>(listaSeguros);
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

        public async Task<BaseResponse<bool>> RegistrarAsegurado(AseguradosRequestDTO request)
        {
            BaseResponse<bool> response = new BaseResponse<bool>();
            try
            {
                Asegurados entidad = _mapper.Map<Asegurados>(request);
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
