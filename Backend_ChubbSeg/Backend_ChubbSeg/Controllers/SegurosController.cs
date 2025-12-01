using Chubbseg.Application.DTOS;
using Chubbseg.Application.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Backend_ChubbSeg.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SegurosController : ControllerBase
    {
        private readonly ISegurosApplication _SegurosApplication;
    
        public SegurosController(ISegurosApplication clientApplication)
        {
            _SegurosApplication = clientApplication;
        
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

        [HttpGet("SegurosID/{id:int}")]
        public async Task<IActionResult> SegurosPorid(int id)
        {
            var response = await _SegurosApplication.SegurosporID(id);
            return Ok(response);
        }

        [HttpPost("RegistrarSeguro")]
        public async Task<IActionResult> RegistrarSeguro([FromBody] SegurosRequestDTO requestDTO)
        {
            var response = await _SegurosApplication.RegistrarSeguro(requestDTO);

            return Ok(response);
        }

        [HttpPut("EditarSeguro/{Id:int}")]
        public async Task<IActionResult> EditarSeguro(int Id, [FromBody] SegurosRequestDTO requestDTO)
        {
            var response = await _SegurosApplication.EditarSeguros(Id,requestDTO);

            return Ok(response);
        }

        [HttpDelete("EliminarSeguro/{Id:int}")]
        public async Task<IActionResult> EliminarSeguro(int Id)
        {
            var response = await _SegurosApplication.EliminarSeguros(Id);
            return Ok(response);
        }
    }
}
