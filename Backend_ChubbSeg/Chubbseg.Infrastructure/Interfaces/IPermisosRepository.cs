using Chubbseg.Domain.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chubbseg.Infrastructure.Interfaces
{
    public interface IPermisosRepository
    {
        Task<List<RolesPermisos>> GetByIdAsync(int id);
    }
}
