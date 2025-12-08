using Chubbseg.Domain.Entidades;
using Chubbseg.Infrastructure.Commons.Request;
using Chubbseg.Infrastructure.Data;
using Chubbseg.Infrastructure.Interfaces;
using Chubbseg.Utilities.Helpers;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chubbseg.Infrastructure.Repositories
{
    public class AuthRepository : IAuthRepository
    {

        private readonly DbContextADO _context;

        public AuthRepository(DbContextADO context)
        {
            _context = context;
  
        }

        public async Task<Login> Auth(AuthRequest auth)
        {
            Login result = new Login();

            using (SqlConnection con = _context.CreateConnection())
            {
                using (SqlCommand cmd = new SqlCommand("LoginUsuario", con))
                {
                    await con.OpenAsync();
                    cmd.CommandType = CommandType.StoredProcedure;

                    // Parámetros
                    cmd.Parameters.AddWithValue("@Credenciales", auth.Credenciales);
                    cmd.Parameters.AddWithValue("@Contraseña", auth.Contrasena);

                    // Parámetro de retorno del SP
                    SqlParameter returnParameter = cmd.Parameters.Add("@return_value", SqlDbType.Int);
                    returnParameter.Direction = ParameterDirection.ReturnValue;
                    using (SqlDataReader reader = await cmd.ExecuteReaderAsync())
                    {
                        if (reader.HasRows)
                        {
                            while (await reader.ReadAsync())
                            {
                                result.IdUsuario = reader.GetInt32(reader.GetOrdinal("IdUsuario"));
                                result.NombreUsuario = reader.GetString(reader.GetOrdinal("NombreUsuario"));
                                result.Correo = reader.GetString(reader.GetOrdinal("Correo"));
                                result.Estado = reader.GetInt32(reader.GetOrdinal("Estado"));
                                result.IdRol = reader.GetInt32(reader.GetOrdinal("IdRol"));
                                result.NombreRol = reader.GetString(reader.GetOrdinal("NombreRol"));
                            }
                        }
                    }

                    result.Resultado = (int)returnParameter.Value;

                    return result;
                }
            }
        }
    }
}
