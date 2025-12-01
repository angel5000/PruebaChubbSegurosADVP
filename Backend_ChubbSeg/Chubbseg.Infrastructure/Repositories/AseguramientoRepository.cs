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
    public class AseguramientoRepository : IAseguramientoRepository
    {
        private readonly DbContextADO _context;

        public AseguramientoRepository(DbContextADO context)
        {
            _context = context;
        }
        async public Task<int> CreateAsync(Aseguramiento aseguramiento)
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
                using (SqlCommand cmd = new SqlCommand("ELIMINARASEGURAMIENTO", con))
                {
                    await con.OpenAsync();
                    cmd.CommandType = CommandType.StoredProcedure;

                    // Parámetros del SP
                    cmd.Parameters.AddWithValue("@IDUSRSEGUROS", id);

                    var returnParameter = cmd.Parameters.Add("@resultado", SqlDbType.Int);
                    returnParameter.Direction = ParameterDirection.ReturnValue;

                    await cmd.ExecuteNonQueryAsync();

                    return (int)returnParameter.Value;
                }
            }
        }

        async public Task<List<Aseguramiento>> GetAllAsync()
        {
            var lista = new List<Aseguramiento>();
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
                        IDUSRSEGUROS = (int)reader["IDUSRSEGUROS"],
                        NMBRSEGURO = (string)reader["NMBRSEGURO"],
                        CODSEGURO = (string)reader["CODSEGURO"],
                        SUMASEGURADA = (decimal)reader["SUMASEGURADA"],
                        PRIMA = (decimal)reader["PRIMA"],
                        CEDULA = (string)reader["CEDULA"],
                        NMBRCOMPLETO = (string)reader["NMBRCOMPLETO"],
                        EDAD = (int)reader["EDAD"],
                        FECHACONTRATASEGURO = (DateTime)reader["FECHACONTRATASEGURO"]
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
