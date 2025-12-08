using Chubbseg.Application.DTOS;
using Chubbseg.Application.Interfaces;
using Chubbseg.Application.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Backend_ChubbSeg.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AseguramientosController : ControllerBase
    {
        private readonly IAseguramientosApplication _AseguramientosApplication;
        private readonly ICargarExcel CARGAexcel;
        public AseguramientosController(IAseguramientosApplication clientApplication, ICargarExcel cARGAexcel)
        {
            _AseguramientosApplication = clientApplication;
            CARGAexcel = cARGAexcel;
        }

        [Authorize]
        [HttpGet("ConsultaAseguramientos")]
        public async Task<IActionResult> ConsultaAseguramientos()
        {
            var response = await _AseguramientosApplication.ListaAseguramientos();

            return Ok(response);
        }

        [Authorize]
        [HttpPost("RegistrarAseguramiento")]
        public async Task<IActionResult> RegistrarSeguramiento([FromBody] AseguramientoRequestDTO requestDTO)
        {
            var response = await _AseguramientosApplication.RegistrarAseguramiento(requestDTO);

            return Ok(response);
        }

        [Authorize]
        [HttpPost("RegistrarAseguramientoMasivo")]
        [Consumes("multipart/form-data")]
  
        public async Task<IActionResult> RegistrarSeguramientomasivo( IFormFile archivo, [FromServices] ICargarExcel subirexcel)
        {
            var response = await CARGAexcel.ProcesarArchivo<AseguradosRequestDTO>(archivo);

            return Ok(response);
        }

        [Authorize]
        [HttpDelete("EliminarAseguramiento/{Id:int}")]
        public async Task<IActionResult> EliminarAseguramiento(int Id, [FromQuery] string usuario)
        {
            var response = await _AseguramientosApplication.EliminarAseguramiento(Id, usuario);
            return Ok(response);
        }
    }
}
