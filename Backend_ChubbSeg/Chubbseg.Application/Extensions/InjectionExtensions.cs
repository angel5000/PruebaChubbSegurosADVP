using Chubbseg.Application.Interfaces;
using Chubbseg.Application.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Chubbseg.Application.Extensions
{
    public static class InjectionExtensions
    {
        public static IServiceCollection AddInjectionApplication(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddSingleton(configuration);


            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

            //services.AddAutoMapper(Assembly.GetExecutingAssembly());
            services.AddScoped<ISegurosApplication, SegurosApplication>();
            services.AddScoped<IAseguradosApplication, AseguradosApplication>();
            services.AddScoped<IAseguramientosApplication, AseguramientosApplication>();
            services.AddScoped<ICargarExcel, CargarExcel>();
            services.AddScoped<ILogin, Login>();
            services.AddScoped<IToken,TokenApplication>();
            services.AddScoped<ICobranzasApplication, CobranzasApplication>();
            services.AddScoped<IRolesPermisosApplication, RolesPermisosApplication>();
            return services;

        }

    }
}
