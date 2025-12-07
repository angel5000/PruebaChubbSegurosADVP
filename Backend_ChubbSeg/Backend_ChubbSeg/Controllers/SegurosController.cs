using Chubbseg.Application.DTOS;
using Chubbseg.Application.Interfaces;
using Chubbseg.Domain.Entidades;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

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

        [HttpGet("ConsultaSeguros")]
        public async Task<IActionResult> ConsultaSeguros()
        {
            var response = await _SegurosApplication.ListaSeguros();
          
            return Ok(response);
        }

        [HttpGet("SegurosDisponibles/{edad:int}")]
        public async Task<IActionResult> SegurosDisponibles(int edad)
        {
            var response = await _SegurosApplication.SegurosDisponibles(edad);
            return Ok(response);
        }

        [HttpGet("CantAsegurados/{id:int}")]
        public async Task<IActionResult> AseguradosPorSeguros(int id)
        {
            var response = await _SegurosApplication.AseguradosPorSeguros(id);
            return Ok(response);
        }


        [HttpGet("SegurosID/{id:int}")]
        public async Task<IActionResult> SegurosPorid(int id)
        {
            var response = await _SegurosApplication.SegurosporID(id);
            return Ok(response);
        }

        [HttpPost("RegistrarSeguro")]
        public async Task<IActionResult> RegistrarSeguro([FromBody] SegurosRequestDTO requestDTO)
        {
            var ip = HttpContext.Connection.RemoteIpAddress?.ToString();
        
            var response = await _SegurosApplication.RegistrarSeguro(requestDTO, HttpContext);

            return Ok(response);
        }

        [HttpPost("RegistrarSegurosMasivo")]
        [Consumes("multipart/form-data")]

        public async Task<IActionResult> RegistrarSegurosmasivo(IFormFile archivo, [FromServices] ICargarExcel subirexcel)
        {
            var ip = HttpContext.Connection.RemoteIpAddress?.ToString();
            var response = await CARGAexcel.RegistMasvSeguros<SegurosRequestDTO>(archivo,HttpContext);

            return Ok(response);
        }

        [HttpPut("EditarSeguro/{Id:int}")]
        public async Task<IActionResult> EditarSeguro(int Id, [FromBody] SegurosRequesteditDTO requestDTO)
        {
            var ip = HttpContext.Connection.RemoteIpAddress?.ToString();
            var response = await _SegurosApplication.EditarSeguros(Id,requestDTO, HttpContext);

            return Ok(response);
        }

        [HttpDelete("EliminarSeguro/{Id:int}")]
        public async Task<IActionResult> EliminarSeguro(int Id, [FromBody] SegurosRequestDeleteDTO request)
        {
            var ip = HttpContext.Connection.RemoteIpAddress?.ToString();
            var response = await _SegurosApplication.EliminarSeguros(Id, request, HttpContext);
            return Ok(response);
        }
    }
}
