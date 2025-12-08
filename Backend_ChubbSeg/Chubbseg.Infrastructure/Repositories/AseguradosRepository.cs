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

namespace Chubbseg.Infrastructure.Repositories
{
    public class AseguradosRepository : IAseguradosRepository
    {
        private readonly DbContextADO _context;

        public AseguradosRepository(DbContextADO context) 
        {
            _context = context;
        }
        public async Task<int> CreateAsync(Asegurados asegurados)
        {
            using (SqlConnection con = _context.CreateConnection())
            {
                using (SqlCommand cmd = new SqlCommand("REGSTASEGURADOS", con))
                {
                    await con.OpenAsync();
                    cmd.CommandType = CommandType.StoredProcedure;

                    // Parámetros del SP
                    cmd.Parameters.AddWithValue("@CEDULA", asegurados.CEDULA);
                    cmd.Parameters.AddWithValue("@NMBRCOMPLETO", asegurados.NMBRCOMPLETO);
                    cmd.Parameters.AddWithValue("@TELEFONO", asegurados.TELEFONO);
                    cmd.Parameters.AddWithValue("@EDAD", asegurados.EDAD);

                    SqlParameter returnParameter = cmd.Parameters.Add("@resultado", SqlDbType.Int);
                    returnParameter.Direction = ParameterDirection.ReturnValue;

                    await cmd.ExecuteNonQueryAsync();

                    return (int)returnParameter.Value;
                }
            }
        }

        public async Task<int> DeleteAsync(int id)
        {
            using (SqlConnection con = _context.CreateConnection())
            {
                using (SqlCommand cmd = new SqlCommand("ELIMINARASEGURADO", con))
                {
                    await con.OpenAsync();
                    cmd.CommandType = CommandType.StoredProcedure;

                    // Parámetros del SP
                    cmd.Parameters.AddWithValue("@IDASEGURADOS", id);

                    SqlParameter returnParameter = cmd.Parameters.Add("@resultado", SqlDbType.Int);
                    returnParameter.Direction = ParameterDirection.ReturnValue;

                    await cmd.ExecuteNonQueryAsync();

                    return (int)returnParameter.Value;
                }
            }
        }

        public async Task<List<Asegurados>> GetAllAsync()
        {
            List<Asegurados> lista = new List<Asegurados>();
            using (SqlConnection con = _context.CreateConnection())
            {
                SqlCommand cmd = new SqlCommand("CONSULTAASEGURADOS", con);
                cmd.CommandType = CommandType.StoredProcedure;
                await con.OpenAsync();

                SqlDataReader reader = await cmd.ExecuteReaderAsync();
                while (await reader.ReadAsync())
                {
                    lista.Add(new Asegurados
                    {
                        IDASEGURADOS = (int)reader["IDASEGURADOS"],
                        CEDULA = (string)reader["CEDULA"],
                        NMBRCOMPLETO = (string)reader["NMBRCOMPLETO"],
                        TELEFONO = (string)reader["TELEFONO"],
                        EDAD = (int)reader["EDAD"]
                    });
                }
            }

            return lista;
        }

        public async Task<Asegurados> GetByIdAsync(int id)
        {
            using (SqlConnection con = _context.CreateConnection())
            {
                SqlCommand cmd = new SqlCommand("CONSULASGURADOSEGID", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@IDASEGURADOS", id);
                await con.OpenAsync();

                using (SqlDataReader reader = await cmd.ExecuteReaderAsync())
                {
                    if (await reader.ReadAsync())
                    {
                        return new Asegurados
                        {
                            IDASEGURADOS = (int)reader["IDASEGURADOS"],
                            CEDULA = (string)reader["CEDULA"],
                            NMBRCOMPLETO = (string)reader["NMBRCOMPLETO"],
                            TELEFONO = (string)reader["TELEFONO"],
                            EDAD = (int)reader["EDAD"],
                        };
                    }
                }
            }
            return null;
        }

        public async Task<int> UpdateAsync(int id, Asegurados seguro)
        {
            using (SqlConnection con = _context.CreateConnection())
            {
                using (SqlCommand cmd = new SqlCommand("EDITARASEGURADOS", con))
                {
                    await con.OpenAsync();
                    cmd.CommandType = CommandType.StoredProcedure;

                    // Parámetros del SP
                    cmd.Parameters.AddWithValue("@IDASEGURADOS", id);
                    cmd.Parameters.AddWithValue("@CEDULA", seguro.CEDULA);
                    cmd.Parameters.AddWithValue("@NMBRCOMPLETO", seguro.NMBRCOMPLETO);
                    cmd.Parameters.AddWithValue("@TELEFONO", seguro.TELEFONO);
                    cmd.Parameters.AddWithValue("@EDAD", seguro.EDAD);

                    SqlParameter returnParameter = cmd.Parameters.Add("@resultado", SqlDbType.Int);
                    returnParameter.Direction = ParameterDirection.ReturnValue;

                    await cmd.ExecuteNonQueryAsync();

                    return (int)returnParameter.Value;
                }
            }
        }
    }
}
