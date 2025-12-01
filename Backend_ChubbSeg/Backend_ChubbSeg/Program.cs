using Chubbseg.Application.Extensions;
using Chubbseg.Application.Mappers;
using Chubbseg.Infrastructure.Data;
using Chubbseg.Infrastructure.Extension;
using Microsoft.OpenApi.Models;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        var Configuration = builder.Configuration;
        // Add services to the container.
        var Cors = "Cors";
        builder.Services.AddControllers();
        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        var connString = Configuration.GetConnectionString("Segurosconex");
        Console.WriteLine($"ConnectionString: {connString}");
        builder.Services.AddSingleton<DbContextADO>();
        builder.Services.AddInjectionApplication(Configuration);
        builder.Services.AddInjectionInfrastructure(Configuration);
        builder.Services.AddAutoMapper(typeof(SegurosMapping));
        builder.Services.AddCors(option =>
        {
            option.AddPolicy(name: Cors, builder => { builder.WithOrigins("http://localhost:4200"); builder.AllowAnyMethod(); builder.AllowAnyHeader(); });
        });
      

        builder.Services.AddHttpContextAccessor();
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        var app = builder.Build();
        app.UseCors(Cors);
       
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();

        app.UseAuthorization();

        app.MapControllers();

        app.Run();
    }
}