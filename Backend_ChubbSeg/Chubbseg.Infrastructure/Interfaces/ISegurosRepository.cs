using Chubbseg.Domain.Entidades;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chubbseg.Infrastructure.Interfaces
{
    public interface ISegurosRepository
    {
        Task<List<Seguros>> GetAllAsync();
        Task<List<Seguros>> GetSelectlistAsync(int value);
        Task<Aseguramiento> GetSelectASegAsync(int value);
        Task<Seguros> GetByIdAsync(int id);
        Task<int> CreateAsync(Seguros seguro, HttpContext context);
        Task<int> UpdateAsync(int id,Seguros seguro, HttpContext context);
        Task<int> DeleteAsync(int id, Seguros seguro, HttpContext context);
    }
}
