using Chubbseg.Domain.Entidades;
using Chubbseg.Infrastructure.Commons.Request;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chubbseg.Infrastructure.Interfaces
{
    public interface ICobranzasRepository
    {
        Task<List<CobranzasResponse>> GetAllAsync();
        Task<int> DeleteAsync(int id,string usuario);
    }
}
