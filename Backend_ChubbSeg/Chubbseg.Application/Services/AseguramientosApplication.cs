using AutoMapper;
using Chubbseg.Application.DTOS;
using Chubbseg.Application.Interfaces;
using Chubbseg.Domain.Entidades;
using Chubbseg.Infrastructure.Interfaces;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chubbseg.Application.Services
{
    public class AseguramientosApplication : IAseguramientosApplication
    {
        private readonly IAseguramientoRepository _repo;
        private readonly IMapper _mapper;
        public AseguramientosApplication(IAseguramientoRepository repo, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }

        public async Task<BaseResponse<bool>> EliminarAseguramiento(int AseguramientoID, string usuario)
        {
            BaseResponse<bool> response = new BaseResponse<bool>();
            try
            {
                int result = await _repo.DeleteAsync(AseguramientoID, usuario);

                if (result > 0)
                {
                    response.Data = true;
                    response.IsSucces = true;
                    response.Message = "Aseguramiento eliminado correctamente.";
                }
                else
                {
                    response.Data = false;
                    response.IsSucces = false;
                    response.Message = "No se pudo eliminar el aseguramiento.";
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

        public async Task<BaseResponse<IEnumerable<AseguramientoResponseDTO>>> ListaAseguramientos()
        {
            BaseResponse<IEnumerable<AseguramientoResponseDTO>> response = new BaseResponse<IEnumerable<AseguramientoResponseDTO>>();
            try
            {
                List<Aseguramiento> listaAseguramientos = await _repo.GetAllAsync();
                List<AseguramientoResponseDTO> listaDto = _mapper.Map<List<AseguramientoResponseDTO>>(listaAseguramientos);
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

        public async Task<BaseResponse<bool>> RegistrarAseguramiento(AseguramientoRequestDTO request)
        {
            BaseResponse<bool> response = new BaseResponse<bool>();
            try
            {
                Aseguramiento entidad = _mapper.Map<Aseguramiento>(request);
                int result = await _repo.CreateAsync(entidad);

                if (result > 0)
                {
                    response.Data = true;
                    response.IsSucces = true;
                    response.Message = "Aseguramiento registrado correctamente.";
                }
                else
                {
                    response.Data = false;
                    response.IsSucces = false;
                    response.Message = "No se pudo registrar el aseguramiento.";
                }
            }
            catch (SqlException ex)
            {
                response.Data = false;
                response.IsSucces = false;
                response.Message = ex.Message;
            }
            catch (Exception ex)
            {
                response.Data = false;
                response.IsSucces = false;
                response.Message = ex.Message;
            }

            return response;
        }
    }
    }
