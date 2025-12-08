using AutoMapper;
using Chubbseg.Application.DTOS;
using Chubbseg.Application.Interfaces;
using Chubbseg.Domain.Entidades;
using Chubbseg.Infrastructure.Interfaces;
using Microsoft.Data.SqlClient;

namespace Chubbseg.Application.Services
{
    public class RolesPermisosApplication : IRolesPermisosApplication
    {
        private readonly IPermisosRepository _repo;
        private readonly IMapper _mapper;
        public RolesPermisosApplication(IPermisosRepository repo, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }
        public async Task<BaseResponse<IEnumerable<RolesPermisosResponseDTO>>> Permisos(int ID)
        {
            BaseResponse<IEnumerable<RolesPermisosResponseDTO>> 
            response = new BaseResponse<IEnumerable<RolesPermisosResponseDTO>>();

            try
            {
                List<RolesPermisos> listaPermisos = await _repo.GetByIdAsync(ID);

                List<RolesPermisosResponseDTO> listaDto = _mapper.Map<List<RolesPermisosResponseDTO>>(listaPermisos);

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
                response.Data = null;
                response.IsSucces = false;
                response.Message = ex.Message;
            }

            return response;
        }
    }
}
