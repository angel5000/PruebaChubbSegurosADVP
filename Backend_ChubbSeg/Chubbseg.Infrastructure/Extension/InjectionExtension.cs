using Chubbseg.Infrastructure.Data;
using Chubbseg.Infrastructure.FileExcel;
using Chubbseg.Infrastructure.FileUpload;
using Chubbseg.Infrastructure.Interfaces;
using Chubbseg.Infrastructure.Repositories;
using Chubbseg.Utilities.Helpers;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;


namespace Chubbseg.Infrastructure.Extension
{
    public static class InjectionExtension
    {
        public static IServiceCollection AddInjectionInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
       
         
            services.AddScoped<ISegurosRepository, SegurosRepository>();
            services.AddScoped<ICargaExcel, CargaExcel>();
            services.AddScoped<ICargaTXT, CargaTXT>();
            services.AddTransient<IPTransform>();
            services.AddScoped<IAseguradosRepository, AseguradosRepository>();
            services.AddScoped<IAseguramientoRepository, AseguramientoRepository>();
            services.AddScoped<IAuthRepository,AuthRepository>();
            services.AddScoped<ICobranzasRepository, CobranzasRepository>();
            services.AddScoped<IPermisosRepository, PermisosRepository>();
            return services;
        }

    }
}
