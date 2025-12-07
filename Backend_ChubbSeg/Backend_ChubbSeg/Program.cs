using Chubbseg.Application.Extensions;
using Chubbseg.Application.Mappers;
using Chubbseg.Infrastructure.Data;
using Chubbseg.Infrastructure.Extension;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;

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

        builder.WebHost.UseKestrel();
        builder.Services.AddHttpContextAccessor();
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();
        builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
      .AddJwtBearer(options =>
      {
          options.TokenValidationParameters = new TokenValidationParameters
          {
              ValidateIssuer = true,
              ValidateAudience = true,
              ValidateLifetime = true,
              ValidateIssuerSigningKey = true,
              ValidIssuer = builder.Configuration["Jwt:Issuer"],
              ValidAudience = builder.Configuration["Jwt:Audience"],
              IssuerSigningKey = new SymmetricSecurityKey(
                  Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"])
              )
          };
      });
        builder.Services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new OpenApiInfo { Title = "Mi API", Version = "v1" });

            // Configuración del JWT
            c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                Name = "Authorization",
                Type = SecuritySchemeType.ApiKey,
                Scheme = "Bearer",
                BearerFormat = "JWT",
                In = ParameterLocation.Header,
                Description = "Ingrese 'Bearer' seguido de un espacio y su token JWT"
            });

            c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[] {}
        }
    });
        });
        builder.Services.AddAuthorization();

        var app = builder.Build();
        app.UseCors(Cors);
       
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();

        app.UseAuthentication();
        app.UseAuthorization();
        app.MapControllers();
       

        app.Run();
    }
}