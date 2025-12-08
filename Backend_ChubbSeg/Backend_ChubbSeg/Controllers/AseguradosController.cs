using Chubbseg.Application.DTOS;
using Chubbseg.Application.Interfaces;
using Chubbseg.Application.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Backend_ChubbSeg.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AseguradosController : ControllerBase
    {
        private readonly IAseguradosApplication _Asegurados;

        public AseguradosController(IAseguradosApplication ConsultAsegurados)
        {
            _Asegurados = ConsultAsegurados;

        }
        [Authorize]
        [HttpGet("ConsultaAsegurados")]
        public async Task<IActionResult> ListCliente()
        {
            var response = await _Asegurados.ListaAsegurados();

            return Ok(response);
        }


        [Authorize]
        [HttpGet("AseguradoID/{id:int}")]
        public async Task<IActionResult> AseguradoporId(int id)
        {
            var response = await _Asegurados.AseguradosporID(id);
            return Ok(response);
        }


        [Authorize]
        [HttpPost("RegistrarAsegurado")]
        public async Task<IActionResult> RegistrarAsegurado([FromBody] AseguradosRequestDTO requestDTO)
        {
            var response = await _Asegurados.RegistrarAsegurado(requestDTO);

            return Ok(response);
        }

        [Authorize]
        [HttpPut("EditarAsegurado/{Id:int}")]
        public async Task<IActionResult> EditarAsegurado(int Id, [FromBody] AseguradosEditRequestDTO requestDTO)
        {
            var response = await _Asegurados.EditarAsegurados(Id, requestDTO);

            return Ok(response);
        }

        [Authorize]
        [HttpDelete("EliminarAsegurado/{Id:int}")]
        public async Task<IActionResult> EliminarAsegurado(int Id)
        {
            var response = await _Asegurados.EliminarAsegurados(Id);
            return Ok(response);
        }
    }
}
