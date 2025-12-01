using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chubbseg.Application.DTOS
{
    public class BaseResponse<T>
    {
        public bool IsSucces { get; set; }
        public T? Data { get; set; }
        public string? Message { get; set; }
        public List<string> Messagemultiple { get; set; } = new();
    
    }
}
