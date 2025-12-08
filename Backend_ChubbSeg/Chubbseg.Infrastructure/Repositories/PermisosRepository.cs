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
    public class PermisosRepository : IPermisosRepository
    {
        private readonly DbContextADO _context;

        public PermisosRepository(DbContextADO context)
        {
            _context = context;
        }

        public async Task<List<RolesPermisos>> GetByIdAsync(int id)
        {
            List<RolesPermisos> lista = new List<RolesPermisos>();

            using (SqlConnection con = _context.CreateConnection())
            {
                SqlCommand cmd = new SqlCommand("CONSULTARPERMISOS", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@IDROL", id);

                await con.OpenAsync();

                using (SqlDataReader reader = await cmd.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())   // ← Lee todas las filas correctamente
                    {
                        RolesPermisos permiso = new RolesPermisos
                        {
                            IdRol = reader["IdRol"] != DBNull.Value ? (int)reader["IdRol"] : 0,
                            IdPermiso = reader["IdPermiso"] != DBNull.Value ? (int)reader["IdPermiso"] : 0,
                            Estado = reader["Estado"] != DBNull.Value ? (bool)reader["Estado"] : false,
                            FechaAsignacion = reader["FechaAsignacion"] != DBNull.Value ? (DateTime)reader["FechaAsignacion"] : DateTime.MinValue
                        };

                        lista.Add(permiso);
                    }
                }
            }

            return lista; // Siem
        }
        }
    }
