using Chubbseg.Domain.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chubbseg.Infrastructure.Interfaces
{
    public interface IAseguradosRepository
    {
        Task<List<Asegurados>> GetAllAsync();
        Task<Asegurados> GetByIdAsync(int id);
        Task<int> CreateAsync(Asegurados asegurados);
        Task<int> UpdateAsync(int id ,Asegurados asegurados);
        Task<int> DeleteAsync(int id);
    }
}
