using Chubbseg.Domain.Entidades;
using Chubbseg.Infrastructure.Data;
using Chubbseg.Infrastructure.Interfaces;
using Chubbseg.Utilities.Helpers;
using Microsoft.AspNetCore.Http;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Chubbseg.Infrastructure.Repositories
{
    public class SegurosRepository : ISegurosRepository
    {

        private readonly DbContextADO _context;
        private readonly IPTransform _ipt;

        public SegurosRepository(DbContextADO context, IPTransform ipt)
        {
            _context = context;
            _ipt = ipt;
        }
        async  public Task<int> CreateAsync(Seguros seguro, HttpContext context)
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
                    //auditoria
                    cmd.Parameters.AddWithValue("@USRCreacion", seguro.USRCreacion ?? "SYSTEM");
                    string? rawIp = context.Connection.RemoteIpAddress?.ToString();

                    if (context.Request.Headers.ContainsKey("X-Forwarded-For"))
                        rawIp = context.Request.Headers["X-Forwarded-For"].ToString();
                    string ipv4 = _ipt.NormalizeIp(rawIp);

                    cmd.Parameters.AddWithValue("@UsuarioIP", ipv4 ?? "0.0.0.0");
                    cmd.Parameters.AddWithValue("@Estado", seguro.Estado);
                    SqlParameter returnParameter = cmd.Parameters.Add("@resultado", SqlDbType.Int);
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

      async public Task<int> UpdateAsync(int id, Seguros seguro, HttpContext context)
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

                    //auditoria
                    cmd.Parameters.AddWithValue("@USRActualizacion", seguro.USRActualizacion ?? "SYSTEM");
                    string? rawIp = context.Connection.RemoteIpAddress?.ToString();

                    if (context.Request.Headers.ContainsKey("X-Forwarded-For"))
                        rawIp = context.Request.Headers["X-Forwarded-For"].ToString();
                    string ipv4 = _ipt.NormalizeIp(rawIp);

                    cmd.Parameters.AddWithValue("@UsuarioIP", ipv4 ?? "0.0.0.0");
                    cmd.Parameters.AddWithValue("@Estado", seguro.Estado);
                    SqlParameter returnParameter = cmd.Parameters.Add("@resultado", SqlDbType.Int);
                    returnParameter.Direction = ParameterDirection.ReturnValue;

                    await cmd.ExecuteNonQueryAsync();

                    return (int)returnParameter.Value; ;
                }
            }
        }

       async public Task<int> DeleteAsync(int id, Seguros seguro, HttpContext context)
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
                    string? rawIp = context.Connection.RemoteIpAddress?.ToString();

                    if (context.Request.Headers.ContainsKey("X-Forwarded-For"))
                        rawIp = context.Request.Headers["X-Forwarded-For"].ToString();
                    string ipv4 = _ipt.NormalizeIp(rawIp);

                    cmd.Parameters.AddWithValue("@UsuarioIP", ipv4 ?? "0.0.0.0");
                    cmd.Parameters.AddWithValue("@EstadoDT", seguro.EstadoDT);


                    SqlParameter returnParameter = cmd.Parameters.Add("@resultado", SqlDbType.Int);
                    returnParameter.Direction = ParameterDirection.ReturnValue;

                    
                    await cmd.ExecuteNonQueryAsync();

                    return (int)returnParameter.Value; ;
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

       async public Task<Aseguramiento> GetSelectASegAsync(int value)
        {
            var lista = new Aseguramiento();
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
