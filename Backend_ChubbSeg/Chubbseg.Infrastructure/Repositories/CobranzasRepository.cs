using Chubbseg.Domain.Entidades;
using Chubbseg.Infrastructure.Commons.Request;
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
    public class CobranzasRepository : ICobranzasRepository
    {
        private readonly DbContextADO _context;

        public CobranzasRepository(DbContextADO context)
        {
            _context = context;
        }

       async public Task<int> DeleteAsync(int id, string usuario)
        {
            try
            {
                using (SqlConnection con = _context.CreateConnection())
            {
                using (SqlCommand cmd = new SqlCommand("CANCELAR_POLIZA", con))
                {
                    await con.OpenAsync();
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@IDUSRSEGUROS", id);
                    cmd.Parameters.AddWithValue("@USUARIO_ANULA", usuario);
                    SqlParameter returnParameter = cmd.Parameters.Add("@resultado", SqlDbType.Int);
                    returnParameter.Direction = ParameterDirection.ReturnValue;
                    await cmd.ExecuteNonQueryAsync();
                    return (int)returnParameter.Value; ;
                }
            }
            }
            catch (SqlException ex)
            {
                throw new Exception("SQL Error: " + ex.Message);
            }
        }

        public async Task<List<CobranzasResponse>> GetAllAsync()
        {
            List<CobranzasResponse> lista = new List<CobranzasResponse>();

            using (SqlConnection con = _context.CreateConnection())
            {
                using (SqlCommand cmd = new SqlCommand("COBRANZASTOTALES", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    await con.OpenAsync();

                    using (SqlDataReader reader = await cmd.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            lista.Add(new CobranzasResponse
                            {
                                IDCOBRANZA = (int)reader["IDCOBRANZA"],
                                IDASEGURADO = (int)reader["IDASEGURADO"],
                                CLIENTE = (string)reader["CLIENTE"],
                                POLIZA = (string)reader["POLIZA"],

                                // Conversión explícita de DateTime a DateOnly (Requiere .NET 6+)
                                FECHA_VENCIMIENTO = DateOnly.FromDateTime((DateTime)reader["FECHA_VENCIMIENTO"]),

                                MONTO_ESPERADO = (decimal)reader["MONTO_ESPERADO"],
                                ESTADO_COBRANZA = (string)reader["ESTADO_COBRANZA"],
                                ESTADO_CALCULADO = (string)reader["ESTADO_CALCULADO"],

                                // Convertir a string de forma segura
                                DIAS_RETRASO = reader["DIAS_RETRASO"].ToString()
                            });
                        }
                    }
                }
            }

            return lista;
        }
    }
}
