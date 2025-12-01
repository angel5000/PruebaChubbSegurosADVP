# PruebaChubbSegurosADVP
| Herramienta                         | Versión recomendada | Enlace                                                                       |
| ----------------------------------- | ------------------- | ---------------------------------------------------------------------------- |
| Node.js                             | >= 22.x             | [https://nodejs.org](https://nodejs.org)                                     |
| Angular CLI                         | >= 17.x             | `npm install -g @angular/cli`                                                |
| .NET SDK                            | >= 8.0              | [https://dotnet.microsoft.com](https://dotnet.microsoft.com)                 |
| SQL Server                          | 2019 o superior     | [https://www.microsoft.com/sql-server](https://www.microsoft.com/sql-server) |
| SQL Server Management Studio (SSMS) | —                   | [https://aka.ms/ssms](https://aka.ms/ssms)                                   |

#
Configuración de la Base de Datos (SQL Server)
#
Ejecutar los scripts puesto en el repositorio
#
Primer script - Crea la base de datos y las tablas con datos
#
SQLCHUBBSEG.sql
Segundo script - crea los sp 
SP_SQLCHUBBSEG.sql
#
Ejecución del Backend (.NET)
#
El proyecto incluye todas las liberias, si hay problemas con ellas ejecutar el comando 
#
dotnet restore
#
-Configurar la cadena de conexion de appsetting.json y development, ubicado en el APiweb backend_chubbseg 
#
-si su base de datos usa usarios diferente al de windows agregar la linea User Id=USUARIO;Password=CLAVE;
#
"ConnectionStrings": {
  "DefaultConnection": "Server=TU_SERVER;Database=TU_DB;TrustServerCertificate=True;"
}
#
-Proceda a ejecutar el programa
#
Puerto configurador 7102
#
Puerto por defecto del fronted por temas de cors http://localhost:4200
#
Ejecución del Frontend (Angular)
#
Dentro de la carpeta Fronted ejecutar el comando desde la consola de visual studio 
#
npm install --force
#
una vez instalada ejecutar ng serve -o 


