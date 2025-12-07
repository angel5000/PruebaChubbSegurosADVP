using Chubbseg.Application.DTOS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chubbseg.Application.Interfaces
{
    public interface IToken
    {
      
      string GenerateToken(LoginResponseDTO response);
        

    }
}
