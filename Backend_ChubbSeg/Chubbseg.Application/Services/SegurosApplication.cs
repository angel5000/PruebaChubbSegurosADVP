using AutoMapper;
using Chubbseg.Application.DTOS;
using Chubbseg.Application.Interfaces;
using Chubbseg.Domain.Entidades;
using Chubbseg.Infrastructure.Interfaces;
using Microsoft.Data.SqlClient;

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

        public async Task<BaseResponse<AseguradosporSeguraresponseDTO>> AseguradosPorSeguros(int id)
        {
            BaseResponse<AseguradosporSeguraresponseDTO> response = new BaseResponse<AseguradosporSeguraresponseDTO>();
            try
            {

                Aseguramiento asegurado = await _repo.GetSelectASegAsync(id);

                AseguradosporSeguraresponseDTO dto = _mapper.Map<AseguradosporSeguraresponseDTO>(asegurado);

                response.Data = dto;
                response.IsSucces = true;
                response.Message = "Consulta realizada correctamente";
            }
            catch (SqlException ex)
            {
                throw new Exception("SQL Error: " + ex.Message);
            }
            catch (Exception ex)
            {
                throw new Exception("Excepcion Error: " + ex.Message);
            }

            return response;
        }

        public async Task<BaseResponse<bool>> EditarSeguros(int SeguroID, SegurosRequesteditDTO request)
        {
            BaseResponse<bool> response = new BaseResponse<bool>();
            try
            {
                Seguros entidad = _mapper.Map<Seguros>(request);
                int result = await _repo.UpdateAsync(SeguroID, entidad);

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
            catch (SqlException ex)
            {
                throw new Exception("SQL Error: " + ex.Message);
            }
            catch (Exception ex)
            {
                throw new Exception("Excepcion Error: " + ex.Message);
            }

            return response;
        }

        public async Task<BaseResponse<bool>> EliminarSeguros(int SeguroID, SegurosRequestDeleteDTO request)
        {
            BaseResponse<bool> response = new BaseResponse<bool>();
            try
            {
                Seguros entidad = _mapper.Map<Seguros>(request);
                int result = await _repo.DeleteAsync(SeguroID, entidad);

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

        public async Task<BaseResponse<IEnumerable<SegurosResponseDTO>>> ListaSeguros()
        {
            BaseResponse<IEnumerable<SegurosResponseDTO>> response = new BaseResponse<IEnumerable<SegurosResponseDTO>>();
            try
            {
                List<Seguros> listaSeguros = await _repo.GetAllAsync();

                List<SegurosResponseDTO> listaDto = _mapper.Map<List<SegurosResponseDTO>>(listaSeguros);

                response.Data = listaDto;
                response.IsSucces = true;
                response.Message = "Consulta realizada correctamente";
            }
            catch (SqlException ex)
            {
                throw new Exception("SQL Error: " + ex.Message);
            }
            catch (Exception ex)
            {
                throw new Exception("Excepcion Error: " + ex.Message);
            }

            return response;
        }

        public async Task<BaseResponse<bool>> RegistrarSeguro(SegurosRequestDTO request)
        {
            BaseResponse<bool> response = new BaseResponse<bool>();
            try
            {
                Seguros entidad = _mapper.Map<Seguros>(request);

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
            catch (SqlException ex)
            {
                throw new Exception("SQL Error: " + ex.Message);
            }
            catch (Exception ex)
            {
                response.Data = false;
                response.IsSucces = false;
                response.Message = ex.Message;
            }

            return response;
        }

        public async Task<BaseResponse<IEnumerable<SegurosResponseDTO>>> SegurosDisponibles(int edad)
        {
            BaseResponse<IEnumerable<SegurosResponseDTO>> response = new BaseResponse<IEnumerable<SegurosResponseDTO>>();
            try
            {

                List<Seguros> listaSeguros = await _repo.GetSelectlistAsync(edad);

                IEnumerable<SegurosResponseDTO> listaDto = _mapper.Map<IEnumerable<SegurosResponseDTO>>(listaSeguros);

                response.Data = listaDto;
                response.IsSucces = true;
                response.Message = "Consulta realizada correctamente";
            }
            catch (SqlException ex)
            {
                throw new Exception("SQL Error: " + ex.Message);
            }
            catch (Exception ex)
            {
                throw new Exception("Excepcion Error: " + ex.Message);
            }

            return response;
        }

        public async Task<BaseResponse<SegurosResponseIDDTO>> SegurosporID(int SeguroID)
        {
            BaseResponse<SegurosResponseIDDTO> response = new BaseResponse<SegurosResponseIDDTO>();
            try
            {
                Seguros seguro = await _repo.GetByIdAsync(SeguroID);

                if (seguro == null)
                {
                    response.IsSucces = false;
                    response.Message = "No se encontró un seguro con ese ID.";
                    return response;
                }

                SegurosResponseIDDTO dto = _mapper.Map<SegurosResponseIDDTO>(seguro);

                response.Data = dto;
                response.IsSucces = true;
                response.Message = "Consulta realizada correctamente";
            }
            catch (SqlException ex)
            {
                throw new Exception("SQL Error: " + ex.Message);
            }
            catch (Exception ex)
            {
                throw new Exception("Excepcion Error: " + ex.Message);
            }

            return response;
        }
    }
}
