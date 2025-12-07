using Chubbseg.Application.DTOS;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chubbseg.Application.Interfaces
{
   public interface ISegurosApplication
    {
        Task<BaseResponse<IEnumerable<SegurosResponseDTO>>> ListaSeguros();
        Task<BaseResponse<SegurosResponseIDDTO>> SegurosporID(int SeguroID);
        Task<BaseResponse<bool>> RegistrarSeguro(SegurosRequestDTO request, HttpContext context);
        Task<BaseResponse<bool>> EditarSeguros(int SeguroID, SegurosRequesteditDTO request, HttpContext context);
        Task<BaseResponse<bool>> EliminarSeguros(int SeguroID, SegurosRequestDeleteDTO request, HttpContext context);
        Task<BaseResponse<IEnumerable<SegurosResponseDTO>>> SegurosDisponibles(int edad);
        Task<BaseResponse<AseguradosporSeguraresponseDTO>> AseguradosPorSeguros(int id);
    }
}
