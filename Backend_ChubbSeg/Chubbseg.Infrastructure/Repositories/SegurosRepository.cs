using Chubbseg.Domain.Entidades;
using Chubbseg.Infrastructure.Data;
using Chubbseg.Infrastructure.Interfaces;
using Chubbseg.Utilities.Helpers;
using Microsoft.Data.SqlClient;
using System.Data;

namespace Chubbseg.Infrastructure.Repositories
{
    public class SegurosRepository : ISegurosRepository
    {
        private readonly DbContextADO _context;
  
        public SegurosRepository(DbContextADO context)
        {
            _context = context;
            
        }
        public async Task<int> CreateAsync(Seguros seguro)
        {
            try
            {
                using (SqlConnection con = _context.CreateConnection())
                {
                    using (SqlCommand cmd = new SqlCommand("REGITSSEGUROS", con))
                    {
                        await con.OpenAsync();
                        cmd.CommandType = CommandType.StoredProcedure;

                        // Parámetros del SP
                        cmd.Parameters.AddWithValue("@NMBRSEGURO", seguro.NMBRSEGURO);
                        cmd.Parameters.AddWithValue("@CODSEGURO", seguro.CODSEGURO);
                        cmd.Parameters.AddWithValue("@SUMASEGURADA", seguro.SUMASEGURADA);
                        cmd.Parameters.AddWithValue("@PRIMA", seguro.PRIMA);
                        cmd.Parameters.AddWithValue("@EDADMIN", seguro.EDADMIN);
                        cmd.Parameters.AddWithValue("@EDADMAX", seguro.EDADMAX);
                        // Auditoria
                        cmd.Parameters.AddWithValue("@USRCreacion", seguro.USRCreacion ?? "SYSTEM");
                        cmd.Parameters.AddWithValue("@UsuarioIP", seguro.UsuarioIP);
                        cmd.Parameters.AddWithValue("@Estado", seguro.Estado);

                        SqlParameter returnParameter = cmd.Parameters.Add("@resultado", SqlDbType.Int);
                        returnParameter.Direction = ParameterDirection.ReturnValue;

                        await cmd.ExecuteNonQueryAsync();

                        return (int)returnParameter.Value;
                    }
                }
            }
            catch (SqlException ex)
            {
                throw new Exception("SQL Error: " + ex.Message);
            }
        }

        public async Task<List<Seguros>> GetAllAsync()
        {
            List<Seguros> lista = new List<Seguros>();
            using (SqlConnection con = _context.CreateConnection())
            {
                SqlCommand cmd = new SqlCommand("CONSULTASEGUROS", con);
                cmd.CommandType = CommandType.StoredProcedure;
                await con.OpenAsync();

                SqlDataReader reader = await cmd.ExecuteReaderAsync();
                while (await reader.ReadAsync())
                {
                    lista.Add(new Seguros
                    {
                        IDSEGURO = reader.GetInt32(reader.GetOrdinal("IDSEGURO")),
                        NMBRSEGURO = reader.GetString(reader.GetOrdinal("NMBRSEGURO")),
                        CODSEGURO = reader.GetString(reader.GetOrdinal("CODSEGURO")).ToLower(),
                        SUMASEGURADA = reader.GetDecimal(reader.GetOrdinal("SUMASEGURADA")),
                        PRIMA = reader.GetDecimal(reader.GetOrdinal("PRIMA")),
                        EDADMIN = reader.GetInt32(reader.GetOrdinal("EDADMIN")),
                        EDADMAX = reader.GetInt32(reader.GetOrdinal("EDADMAX")),
                        FechaCreacion = reader.GetDateTime(reader.GetOrdinal("FechaCreacion")),
                        USRCreacion = reader.GetString(reader.GetOrdinal("USRCreacion")),
                        FechaActualizacion = reader.IsDBNull(reader.GetOrdinal("FechaActualizacion"))
                            ? (DateTime?)null
                            : reader.GetDateTime(reader.GetOrdinal("FechaActualizacion")),
                        USRActualizacion = reader.IsDBNull(reader.GetOrdinal("USRActualizacion"))
                            ? null
                            : reader.GetString(reader.GetOrdinal("USRActualizacion")),
                        UsuarioIP = reader.IsDBNull(reader.GetOrdinal("UsuarioIP"))
                            ? null
                            : reader.GetString(reader.GetOrdinal("UsuarioIP")),
                        Estado = reader.GetBoolean(reader.GetOrdinal("Estado"))
                    });
                }
            }

            return lista;
        }

        public async Task<Seguros> GetByIdAsync(int id)
        {
            // Nota: 'lista' no se usaba en el código original para este método, se eliminó para limpiar.
            using (SqlConnection con = _context.CreateConnection())
            {
                SqlCommand cmd = new SqlCommand("CONSULSEGID", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@IDSEGURO", id);
                await con.OpenAsync();

                using (SqlDataReader reader = await cmd.ExecuteReaderAsync())
                {
                    if (await reader.ReadAsync())
                    {
                        return new Seguros
                        {
                            IDSEGURO = (int)reader["IDSEGURO"],
                            NMBRSEGURO = (string)reader["NMBRSEGURO"],
                            CODSEGURO = (string)reader["CODSEGURO"],
                            SUMASEGURADA = (decimal)reader["SUMASEGURADA"],
                            PRIMA = (decimal)reader["PRIMA"],
                            EDADMIN = (int)reader["EDADMIN"],
                            EDADMAX = (int)reader["EDADMAX"],
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
                        };
                    }
                }
            }
            return null;
        }

        public async Task<int> UpdateAsync(int id, Seguros seguro)
        {
            using (SqlConnection con = _context.CreateConnection())
            {
                using (SqlCommand cmd = new SqlCommand("EDITARSEGUROS", con))
                {
                    await con.OpenAsync();
                    cmd.CommandType = CommandType.StoredProcedure;

                    // Parámetros del SP
                    cmd.Parameters.AddWithValue("@IDSEGURO", id);
                    cmd.Parameters.AddWithValue("@NMBRSEGURO", seguro.NMBRSEGURO);
                    cmd.Parameters.AddWithValue("@CODSEGURO", seguro.CODSEGURO);
                    cmd.Parameters.AddWithValue("@SUMASEGURADA", seguro.SUMASEGURADA);
                    cmd.Parameters.AddWithValue("@PRIMA", seguro.PRIMA);
                    cmd.Parameters.AddWithValue("@EDADMIN", seguro.EDADMIN);
                    cmd.Parameters.AddWithValue("@EDADMAX", seguro.EDADMAX);

                    // Auditoria
                    cmd.Parameters.AddWithValue("@USRActualizacion", seguro.USRActualizacion ?? "SYSTEM");
                    cmd.Parameters.AddWithValue("@UsuarioIP", seguro.UsuarioIP);
                    cmd.Parameters.AddWithValue("@Estado", seguro.Estado);

                    SqlParameter returnParameter = cmd.Parameters.Add("@resultado", SqlDbType.Int);
                    returnParameter.Direction = ParameterDirection.ReturnValue;

                    await cmd.ExecuteNonQueryAsync();

                    return (int)returnParameter.Value;
                }
            }
        }

        public async Task<int> DeleteAsync(int id, Seguros seguro)
        {
            using (SqlConnection con = _context.CreateConnection())
            {
                using (SqlCommand cmd = new SqlCommand("ELIMINARSEGURO", con))
                {
                    await con.OpenAsync();
                    cmd.CommandType = CommandType.StoredProcedure;

                    // Parámetros del SP
                    cmd.Parameters.AddWithValue("@IDSEGURO", id);
                    cmd.Parameters.AddWithValue("@USRActualizacion", seguro.USRActualizacion ?? "SYSTEM");
                    cmd.Parameters.AddWithValue("@UsuarioIP", seguro.UsuarioIP);
                    cmd.Parameters.AddWithValue("@EstadoDT", seguro.EstadoDT);

                    SqlParameter returnParameter = cmd.Parameters.Add("@resultado", SqlDbType.Int);
                    returnParameter.Direction = ParameterDirection.ReturnValue;

                    await cmd.ExecuteNonQueryAsync();

                    return (int)returnParameter.Value;
                }
            }
        }

        public async Task<List<Seguros>> GetSelectlistAsync(int value)
        {
            List<Seguros> lista = new List<Seguros>();
            try
            {
                using (SqlConnection con = _context.CreateConnection())
                {
                    using (SqlCommand cmd = new SqlCommand("SEGUR0SDISPO", con))
                    {
                        await con.OpenAsync();
                        cmd.CommandType = CommandType.StoredProcedure;

                        // Parámetro del SP
                        cmd.Parameters.AddWithValue("@Edad", value);

                        // Ejecutar el SP
                        using (SqlDataReader reader = await cmd.ExecuteReaderAsync())
                        {
                            if (!reader.HasRows)
                            {
                                return new List<Seguros>();
                            }

                            while (await reader.ReadAsync())
                            {
                                Seguros seguro = new Seguros
                                {
                                    IDSEGURO = (int)reader["IDSEGURO"],
                                    CODSEGURO = (string)reader["CODSEGURO"],
                                    NMBRSEGURO = (string)reader["NMBRSEGURO"],
                                    EDADMIN = (int)reader["EDADMIN"],
                                    EDADMAX = (int)reader["EDADMAX"],
                                    SUMASEGURADA = (decimal)reader["SUMASEGURADA"],
                                    PRIMA = (decimal)reader["PRIMA"]
                                };

                                lista.Add(seguro);
                            }
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
            return lista;
        }

        public async Task<Aseguramiento> GetSelectASegAsync(int value)
        {
            using (SqlConnection con = _context.CreateConnection())
            {
                SqlCommand cmd = new SqlCommand("ConsulAseguradosPorSeguros", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@IDSEGURO", value);
                await con.OpenAsync();

                using (SqlDataReader reader = await cmd.ExecuteReaderAsync())
                {
                    if (await reader.ReadAsync())
                    {
                        return new Aseguramiento
                        {
                            IDASEGURADOS = (int)reader["IDASEGURADOS"],
                            CEDULA = (string)reader["CEDULA"],
                            NMBRCOMPLETO = (string)reader["NMBRCOMPLETO"],
                            TELEFONO = (string)reader["TELEFONO"],
                            EDAD = (int)reader["EDAD"],
                            FECHACONTRATASEGURO = (string)reader["FECHACONTRATASEGURO"]
                        };
                    }
                }
            }
            return null;
        }
    }
}
