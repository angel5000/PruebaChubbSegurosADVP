using Chubbseg.Application.DTOS;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chubbseg.Application.Interfaces
{
    public interface ICargarExcel
    {
        Task<BaseResponse<bool>> ProcesarArchivo<T> (IFormFile archivo) where T : class, new();
        Task<BaseResponse<bool>> RegistMasvSeguros<T>(IFormFile archivo) where T : class, new();
    }
}
