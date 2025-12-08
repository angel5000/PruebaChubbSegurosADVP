using Chubbseg.Domain.Entidades;
using Chubbseg.Infrastructure.Data;
using Chubbseg.Infrastructure.Interfaces;
using Microsoft.Data.SqlClient;
using System.Data;

namespace Chubbseg.Infrastructure.Repositories
{
    public class AseguramientoRepository : IAseguramientoRepository
    {
        private readonly DbContextADO _context;

        public AseguramientoRepository(DbContextADO context)
        {
            _context = context;
        }
        public async Task<int> CreateAsync(Aseguramiento aseguramiento)
        {
            using (SqlConnection con = _context.CreateConnection())
            {
                using (SqlCommand cmd = new SqlCommand("REGASEGURAMIENTO", con))
                {
                    await con.OpenAsync();
                    cmd.CommandType = CommandType.StoredProcedure;

                    // Parámetros del SP
                    cmd.Parameters.AddWithValue("@CEDULA", aseguramiento.CEDULA);
                    cmd.Parameters.AddWithValue("@CODSEGURO", aseguramiento.CODSEGURO);

                    SqlParameter returnParameter = cmd.Parameters.Add("@resultado", SqlDbType.Int);
                    returnParameter.Direction = ParameterDirection.ReturnValue;

                    await cmd.ExecuteNonQueryAsync();

                    return (int)returnParameter.Value;
                }
            }
        }

        public async Task<int> DeleteAsync(int id, string usuario)
        {
            using (SqlConnection con = _context.CreateConnection())
            {
                using (SqlCommand cmd = new SqlCommand("ELIMINARASEGURAMIENTO", con))
                {
                    await con.OpenAsync();
                    cmd.CommandType = CommandType.StoredProcedure;

                    // Parámetros del SP
                    cmd.Parameters.AddWithValue("@IDUSRSEGUROS", id);
                    cmd.Parameters.AddWithValue("@USUARIO_EJECUTA", usuario);

                    SqlParameter returnParameter = cmd.Parameters.Add("@resultado", SqlDbType.Int);
                    returnParameter.Direction = ParameterDirection.ReturnValue;

                    await cmd.ExecuteNonQueryAsync();

                    return (int)returnParameter.Value;
                }
            }
        }

        public async Task<List<Aseguramiento>> GetAllAsync()
        {
            List<Aseguramiento> lista = new List<Aseguramiento>();
            using (SqlConnection con = _context.CreateConnection())
            {
                SqlCommand cmd = new SqlCommand("CONSULTASEGURAMIENTO", con);
                cmd.CommandType = CommandType.StoredProcedure;
                await con.OpenAsync();

                SqlDataReader reader = await cmd.ExecuteReaderAsync();
                while (await reader.ReadAsync())
                {
                    lista.Add(new Aseguramiento
                    {
                        IDASEGURADOS = (int)reader["IDASEGURADOS"],
                        CEDULA = (string)reader["CEDULA"],
                        NMBRSEGURO = (string)reader["NMBRSEGURO"],
                        EDAD = (int)reader["EDAD"],

                        IDUSRSEGUROS = (int)reader["IDUSRSEGUROS"],
                        CODSEGURO = (string)reader["CODSEGURO"],
                        SUMASEGURADA = (decimal)reader["SUMASEGURADA"],
                        PRIMA = (decimal)reader["PRIMA"],
                        NMBRCOMPLETO = (string)reader["NMBRCOMPLETO"],

                        FECHACONTRATASEGURO = reader["FECHACONTRATASEGURO"] == DBNull.Value
                            ? null
                            : (string)reader["FECHACONTRATASEGURO"],

                        USRCreacion = reader["USRCreacion"] as string,

                        FechaActualizacion = reader["FechaActualizacion"] == DBNull.Value
                            ? (DateTime?)null
                            : (DateTime)reader["FechaActualizacion"],

                        USRActualizacion = reader["USRActualizacion"] == DBNull.Value
                            ? null
                            : (string)reader["USRActualizacion"],

                        UsuarioIP = reader["UsuarioIP"] == DBNull.Value
                            ? null
                            : (string)reader["UsuarioIP"],

                        Estado = reader["Estado"] == DBNull.Value
                            ? (bool?)null
                            : (bool)reader["Estado"],
                    });
                }
            }

            return lista;
        }
        public Task<Aseguramiento> GetByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<int> UpdateAsync(int id, Aseguramiento aseguramiento)
        {
            throw new NotImplementedException();
        }
    }
}
