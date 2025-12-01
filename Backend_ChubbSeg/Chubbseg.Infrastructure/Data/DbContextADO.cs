using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;


namespace Chubbseg.Infrastructure.Data
{
    public class DbContextADO
    {
        private readonly string _connectionString;

        public DbContextADO(IConfiguration configuration)
        {
            // Lee la cadena desde appsettings.json
            _connectionString = configuration.GetConnectionString("Segurosconex");
        }

        // Método para obtener la conexión
        public SqlConnection CreateConnection()
        {
            return new SqlConnection(_connectionString);
        }

    }

}
