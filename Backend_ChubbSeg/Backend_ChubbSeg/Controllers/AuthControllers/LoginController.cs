using Chubbseg.Application.DTOS;
using Chubbseg.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Backend_ChubbSeg.Controllers.AuthControllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly ILogin _login;
        public LoginController(ILogin login)
        {
            _login = login;

        }
        [HttpPost("IniciarSesion")]
        public async Task<IActionResult> IniciarSesion([FromBody] LoginDTO requestDTO)
        {
            var response = await _login.IniciarSesion(requestDTO);

            return Ok(response);
        }
    }
}
