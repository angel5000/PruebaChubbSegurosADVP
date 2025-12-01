using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chubbseg.Infrastructure.FileExcel
{
    public interface ICargaExcel
    {
        List<T> SubirExcel<T>(IFormFile file) where T : class, new();
    }
}
