using AutoMapper;
using Azure.Core;
using Chubbseg.Application.DTOS;
using Chubbseg.Application.Interfaces;
using Chubbseg.Domain.Entidades;
using Chubbseg.Infrastructure.Commons.Request;
using Chubbseg.Infrastructure.Interfaces;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chubbseg.Application.Services
{
    public class CobranzasApplication : ICobranzasApplication
    {
        private readonly ICobranzasRepository _repo;
        private readonly IMapper _mapper;

        public CobranzasApplication(ICobranzasRepository repo, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }

        public async Task<BaseResponse<bool>> CancelarSeguro(int AseguradoID, string usuariomod)
        {
            BaseResponse<bool> response = new BaseResponse<bool>();
            try
            {
                int result = await _repo.DeleteAsync(AseguradoID, usuariomod);

                if (result > 0)
                {
                    response.Data = true;
                    response.IsSucces = true;
                    response.Message = "Seguro cancelado correctamente.";
                }
                else
                {
                    response.Data = false;
                    response.IsSucces = false;
                    response.Message = "No se pudo cancelar el seguro.";
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

        public async Task<BaseResponse<IEnumerable<CobranzasResponseDTO>>> ListaCobranzas()
        {
            BaseResponse<IEnumerable<CobranzasResponseDTO>> 
            response = new BaseResponse<IEnumerable<CobranzasResponseDTO>>();
            try
            {
                List<CobranzasResponse> listaSeguros = await _repo.GetAllAsync();
                List<CobranzasResponseDTO> listaDto = _mapper.Map<List<CobranzasResponseDTO>>(listaSeguros);

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
    }
}
