using Chubbseg.Domain.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chubbseg.Infrastructure.Interfaces
{
    public interface IAseguramientoRepository
    {
        Task<List<Aseguramiento>> GetAllAsync();
        Task<Aseguramiento> GetByIdAsync(int id);
        Task<int> CreateAsync(Aseguramiento aseguramiento);
        Task<int> UpdateAsync(int id, Aseguramiento aseguramiento);
        Task<int> DeleteAsync(int id,string usuario);


    }
}
