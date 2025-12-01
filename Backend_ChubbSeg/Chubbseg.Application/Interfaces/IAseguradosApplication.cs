using Chubbseg.Application.DTOS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chubbseg.Application.Interfaces
{
    public interface IAseguradosApplication
    {
        Task<BaseResponse<IEnumerable<AseguradosResponseDTO>>> ListaAsegurados();
        Task<BaseResponse<bool>> RegistrarAsegurado(AseguradosRequestDTO request);
        Task<BaseResponse<bool>> EditarAsegurados(int AseguradoID, AseguradosRequestDTO request);
        Task<BaseResponse<bool>> EliminarAsegurados(int AseguradoID);
        Task<BaseResponse<AseguradosResponseDTO>> AseguradosporID(int AseguradoID);
    }
}
