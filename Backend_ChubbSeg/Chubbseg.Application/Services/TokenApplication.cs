using Chubbseg.Application.DTOS;
using Chubbseg.Application.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Chubbseg.Application.Services
{
    public class TokenApplication : IToken
    {
        private readonly IConfiguration _config;

        public TokenApplication(IConfiguration config)
        {
            _config = config;
        }

        public string GenerateToken(LoginResponseDTO response)
        {
            Claim[] claims = new[]
            {
        new Claim(ClaimTypes.Name, response.NombreUsuario),
        new Claim(ClaimTypes.Role, response.IdRol.ToString()),
        // Asumiendo que Correo ya es string, el .ToString() es redundante pero seguro
        new Claim(ClaimTypes.Email, response.Correo.ToString()),
        new Claim("Estado", response.Estado.ToString())
    };

            SymmetricSecurityKey key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));

            SigningCredentials creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            JwtSecurityToken token = new JwtSecurityToken(
                issuer: _config["Jwt:Issuer"],
                audience: _config["Jwt:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(int.Parse(_config["Jwt:ExpiresInMinutes"])),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
