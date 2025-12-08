using AutoMapper;
using Chubbseg.Application.DTOS;
using Chubbseg.Application.Interfaces;
using Chubbseg.Domain.Entidades;
using Chubbseg.Infrastructure.Commons.Request;
using Chubbseg.Infrastructure.Interfaces;
using OfficeOpenXml.FormulaParsing.LexicalAnalysis;


namespace Chubbseg.Application.Services
{
    public class Login:ILogin
    {
        private readonly IAuthRepository _repo;
        private readonly IMapper _mapper;
        private readonly IToken _token;
        public Login(IAuthRepository repo, IMapper mapper, IToken token)
        {
            _repo = repo;
            _mapper = mapper;
            _token = token;
        }


        async public Task<BaseResponse<LoginResponseDTO>> IniciarSesion(LoginDTO request)
        {
           
            BaseResponse<LoginResponseDTO> response = new BaseResponse<LoginResponseDTO>();
            AuthRequest entidad = _mapper.Map<AuthRequest>(request);

            try
            {
                Chubbseg.Domain.Entidades.Login loginResult = await _repo.Auth(entidad);

                if (loginResult.Resultado == 1)
                {
                    LoginResponseDTO listaDto = _mapper.Map<LoginResponseDTO>(loginResult);
                    string restoken = _token.GenerateToken(listaDto);

                    response.IsSucces = true;
                    listaDto.token = restoken;
                    response.Data = listaDto;
                    response.Message = "Login exitoso.";
                }
                else if (loginResult.Resultado == -1)
                {
                    response.IsSucces = false;
                    response.Message = "Usuario o correo no existe.";
                }
                else if (loginResult.Resultado == -2)
                {
                    response.Message = "Contraseña incorrecta.";
                }
                else if (loginResult.Resultado == -3)
                {
                    response.Message = "Usuario inactivo.";
                }
                else
                {
                    response.Message = "Error desconocido.";
                }
            }
            catch (Exception ex)
            {
                response.IsSucces = false;
                response.Message = ex.Message;
            }

            return response;
        }
    }
}
