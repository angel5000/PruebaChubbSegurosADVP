using Chubbseg.Domain.Entidades;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chubbseg.Infrastructure.FileUpload
{
    public interface ICargaTXT
    {
        List<T> ImportarTxt <T>(IFormFile file) where T : class, new();
    }
}
