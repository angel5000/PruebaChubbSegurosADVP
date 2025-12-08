using Chubbseg.Application.DTOS;
using Chubbseg.Application.Interfaces;
using Chubbseg.Application.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;

namespace Backend_ChubbSeg.Controllers.PermisosController
{
    [Route("api/[controller]")]
    [ApiController]
    public class PermisosController : ControllerBase
    {
        private readonly IRolesPermisosApplication _Permisos;

        public PermisosController(IRolesPermisosApplication Permisos)
        {
            _Permisos = Permisos;

        }

        [Authorize]
        [HttpGet("ConsultarPermisos/{id:int}")]
        public async Task<IActionResult> ConsultaPermisos(int id)
        {
            BaseResponse<IEnumerable<RolesPermisosResponseDTO>> response 
            = new BaseResponse<IEnumerable<RolesPermisosResponseDTO>>();
            try
            {
                BaseResponse<IEnumerable<RolesPermisosResponseDTO>> result = await _Permisos.Permisos(id);
                response = result;
            }
            catch (SqlException ex)
            {
                response.IsSucces = false;
                response.Message = "Hubo un problema en la comunicación con la base de datos. Inténtalo más tarde.";
            }
            catch (Exception ex)
            {
                response.IsSucces = false;
                response.Message = "Hubo un problema en la comunicación con el servidor. Inténtalo más tarde.";

            }
            return Ok(response); ;
        }

    }
}
