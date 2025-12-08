using Chubbseg.Application.DTOS;
using Chubbseg.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;

namespace Backend_ChubbSeg.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SegurosController : ControllerBase
    {
        private readonly ISegurosApplication _SegurosApplication;
        private readonly ICargarExcel CARGAexcel;
        public SegurosController(ISegurosApplication clientApplication, ICargarExcel cARGAexcel)
        {
            _SegurosApplication = clientApplication;
            CARGAexcel = cARGAexcel;
        }

        [Authorize]
        [HttpGet("ConsultaSeguros")]
        public async Task<IActionResult> ConsultaSeguros()
        {
      
            BaseResponse<IEnumerable<SegurosResponseDTO>> response = new BaseResponse<IEnumerable<SegurosResponseDTO>>();
            try
            {
                BaseResponse<IEnumerable<SegurosResponseDTO>> result = await _SegurosApplication.ListaSeguros();
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

        [Authorize]
        [HttpGet("SegurosDisponibles/{edad:int}")]
        public async Task<IActionResult> SegurosDisponibles(int edad)
        {
            BaseResponse <IEnumerable<SegurosResponseDTO>> response = new BaseResponse<IEnumerable<SegurosResponseDTO>>();
            try
            {
                BaseResponse<IEnumerable<SegurosResponseDTO>> result = await _SegurosApplication.SegurosDisponibles(edad);
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

        [Authorize]
        [HttpGet("CantAsegurados/{id:int}")]
        public async Task<IActionResult> AseguradosPorSeguros(int id)
        {
            BaseResponse<AseguradosporSeguraresponseDTO> response = new BaseResponse<AseguradosporSeguraresponseDTO>();
            try
            {
                BaseResponse<AseguradosporSeguraresponseDTO> result = await _SegurosApplication.AseguradosPorSeguros(id);
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

        [Authorize]
        [HttpGet("SegurosID/{id:int}")]
        public async Task<IActionResult> SegurosPorid(int id)
        {
            BaseResponse<SegurosResponseIDDTO> response = new BaseResponse<SegurosResponseIDDTO>();
            try
            {
                BaseResponse<SegurosResponseIDDTO> result = await _SegurosApplication.SegurosporID(id);
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

        [Authorize]
        [HttpPost("RegistrarSeguro")]
        public async Task<IActionResult> RegistrarSeguro([FromBody] SegurosRequestDTO requestDTO)
        {
            BaseResponse<bool> response = new BaseResponse<bool>();
            try
            {
                BaseResponse<bool> result = await _SegurosApplication.RegistrarSeguro(requestDTO);
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

        [Authorize]
        [HttpPost("RegistrarSegurosMasivo")]
        [Consumes("multipart/form-data")]

        public async Task<IActionResult> RegistrarSegurosmasivo(IFormFile archivo, [FromServices] ICargarExcel subirexcel,[FromQuery] string usuario,
        [FromQuery] string ip)
        {
            BaseResponse<bool> response = new BaseResponse<bool>();
            try
            {
                BaseResponse<bool> result = await CARGAexcel.RegistMasvSeguros<SegurosRequestDTO>(archivo, usuario, ip);
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

        [Authorize]
        [HttpPut("EditarSeguro/{Id:int}")]
        public async Task<IActionResult> EditarSeguro(int Id, [FromBody] SegurosRequesteditDTO requestDTO)
        {
        
            BaseResponse<bool> response = new BaseResponse<bool>();
            try
            {
                BaseResponse<bool> result = await _SegurosApplication.EditarSeguros(Id, requestDTO);
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

        [Authorize]
        [HttpDelete("EliminarSeguro/{Id:int}")]
        public async Task<IActionResult> EliminarSeguro(int Id, [FromBody] SegurosRequestDeleteDTO request)
        {
            BaseResponse<bool> response = new BaseResponse<bool>();
            try
            {
                BaseResponse<bool> result = await _SegurosApplication.EliminarSeguros(Id, request);
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
