using Chubbseg.Application.DTOS;
using Chubbseg.Application.Interfaces;
using Chubbseg.Application.Services;
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

        [HttpGet("ConsultaAseguramientos")]
        public async Task<IActionResult> ConsultaAseguramientos()
        {
            var response = await _AseguramientosApplication.ListaAseguramientos();

            return Ok(response);
        }

        [HttpPost("RegistrarAseguramiento")]
        public async Task<IActionResult> RegistrarSeguramiento([FromBody] AseguramientoRequestDTO requestDTO)
        {
            var response = await _AseguramientosApplication.RegistrarAseguramiento(requestDTO);

            return Ok(response);
        }

        [HttpPost("RegistrarAseguramientoMasivo")]
        [Consumes("multipart/form-data")]
  
        public async Task<IActionResult> RegistrarSeguramientomasivo( IFormFile archivo, [FromServices] ICargarExcel subirexcel)
        {
            var response = await CARGAexcel.ProcesarArchivo<AseguradosRequestDTO>(archivo);

            return Ok(response);
        }

        [HttpDelete("EliminarAseguramiento/{Id:int}")]
        public async Task<IActionResult> EliminarAseguramiento(int Id)
        {
            var response = await _AseguramientosApplication.EliminarAseguramiento(Id);
            return Ok(response);
        }
    }
}
