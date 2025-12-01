using Chubbseg.Domain.Entidades;
using Chubbseg.Infrastructure.Data;
using Chubbseg.Infrastructure.Interfaces;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Chubbseg.Infrastructure.Repositories
{
    public class SegurosRepository : ISegurosRepository
    {

        private readonly DbContextADO _context;

        public SegurosRepository(DbContextADO context) 
        {
            _context = context;
        }
      async  public Task<int> CreateAsync(Seguros seguro)
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

                    var returnParameter = cmd.Parameters.Add("@resultado", SqlDbType.Int);
                    returnParameter.Direction = ParameterDirection.ReturnValue;
                
                    await cmd.ExecuteNonQueryAsync();

                    return (int)returnParameter.Value;
                }
            }
        }


        async public Task<List<Seguros>> GetAllAsync()
        {
            var lista = new List<Seguros>();
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
                        EDADMAX = reader.GetInt32(reader.GetOrdinal("EDADMAX"))

                    });
                }
            }

            return lista;
        
        }

       async public Task<Seguros> GetByIdAsync(int id)
        {
            var lista = new List<Seguros>();
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
                            EDADMAX = (int)reader["EDADMAX"]
                        };
                    }
                }
            }
                return null;
            }

      async public Task<int> UpdateAsync(int id, Seguros seguro)
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

                    var returnParameter = cmd.Parameters.Add("@resultado", SqlDbType.Int);
                    returnParameter.Direction = ParameterDirection.ReturnValue;

                    await cmd.ExecuteNonQueryAsync();

                    return (int)returnParameter.Value;
                }
            }
        }

       async public Task<int> DeleteAsync(int id)
        {
            using (SqlConnection con = _context.CreateConnection())
            {
                using (SqlCommand cmd = new SqlCommand("ELIMINARSEGURO", con))
                {
                    await con.OpenAsync();
                    cmd.CommandType = CommandType.StoredProcedure;

                    // Parámetros del SP
                    cmd.Parameters.AddWithValue("@IDSEGURO", id);
                   
                    var returnParameter = cmd.Parameters.Add("@resultado", SqlDbType.Int);
                    returnParameter.Direction = ParameterDirection.ReturnValue;

                    await cmd.ExecuteNonQueryAsync();

                    return (int)returnParameter.Value;
                }
            }
        }

       async public Task<List<Seguros>> GetSelectlistAsync(int value)
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
                            for (int i = 0; i < reader.FieldCount; i++)
                            {
                                Console.WriteLine(reader.GetName(i));
                            }

                            while (await reader.ReadAsync())
                            {
                                Seguros seguro = new Seguros
                                {
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
    }
}
