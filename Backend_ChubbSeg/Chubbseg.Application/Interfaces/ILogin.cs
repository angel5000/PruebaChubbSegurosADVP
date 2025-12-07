using Chubbseg.Application.DTOS;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chubbseg.Application.Interfaces
{
    public interface ILogin
    {
        Task<BaseResponse<LoginResponseDTO>> IniciarSesion(LoginDTO request);

    }
}
