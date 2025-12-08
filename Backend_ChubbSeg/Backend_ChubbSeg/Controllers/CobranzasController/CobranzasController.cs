using Chubbseg.Application.DTOS;
using Chubbseg.Application.Interfaces;
using Chubbseg.Application.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;

namespace Backend_ChubbSeg.Controllers.CobranzasController
{
    [Route("api/[controller]")]
    [ApiController]
    public class CobranzasController : ControllerBase
    {
        private readonly ICobranzasApplication _CobranzasApplication;

        public CobranzasController(ICobranzasApplication cobranzasApplication)
        {
            _CobranzasApplication = cobranzasApplication;
            
        }

        [Authorize]
        [HttpGet("ConsultaCobranzas")]
        public async Task<IActionResult> ConsultarCobranzas()
        {
            var response = await _CobranzasApplication.ListaCobranzas();

            return Ok(response);
        }


        [Authorize]
        [HttpDelete("CancelarSeguro/{Id:int}")]
        public async Task<IActionResult> EliminarSeguro(int Id, [FromQuery] string usuario)
        {
            BaseResponse<bool> response = new BaseResponse<bool>();
            try
            {
                BaseResponse<bool> result = await _CobranzasApplication.CancelarSeguro(Id, usuario);
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
            return Ok(response);
        }
    }
}
