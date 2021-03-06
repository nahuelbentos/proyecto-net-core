using System.Text;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Aplicacion.Contratos;
using Aplicacion.Cursos;
using Dominio;
using FluentValidation.AspNetCore;
using MediatR;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Persistencia;
using Seguridad;
using WebAPI.Middleware;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Authorization;
using AutoMapper;
using Persistencia.DapperConexion;
using Persistencia.DapperConexion.Instructor;
using Microsoft.OpenApi.Models;
using Persistencia.DapperConexion.Paginacion;

namespace WebAPI
{
  public class Startup
  {
    public Startup(IConfiguration configuration)
    {
      Configuration = configuration;
    }

    public IConfiguration Configuration { get; }

    // This method gets called by the runtime. Use this method to add services to the container.
    public void ConfigureServices(IServiceCollection services)
    {
      // Configuración de CORS
      services.AddCors(o => o.AddPolicy("corsApp", builder =>
      {
        builder
          .AllowAnyOrigin()
          .AllowAnyMethod()
          .AllowAnyHeader();
      }));

      services.AddDbContext<CursosOnlineContext>(opt =>
      {
        opt.UseSqlServer(this.Configuration.GetConnectionString("DefaultConnection"));
      });

      services.Configure<ConexionConfiguracion>(this.Configuration.GetSection("ConnectionStrings"));
      services.AddOptions();


      services.AddMediatR(typeof(Consulta.Manejador).Assembly);

      services.AddControllers(opt =>
      {
        // Declaro politica para requerir Autenticación y la agrego como filtro.
        var policy = new AuthorizationPolicyBuilder().RequireAuthenticatedUser().Build();
        opt.Filters.Add(new AuthorizeFilter(policy));
      })
          .AddFluentValidation(config => config.RegisterValidatorsFromAssemblyContaining<Nuevo>());

      // Configuración de IdentityCore
      var builder = services.AddIdentityCore<Usuario>();
      var identityBuilder = new IdentityBuilder(builder.UserType, builder.Services);

      //Configuración de roles y Rol con Tokens -> Ademas instanciamos el servicio de RoleManager
      identityBuilder.AddRoles<IdentityRole>();
      identityBuilder.AddClaimsPrincipalFactory<UserClaimsPrincipalFactory<Usuario, IdentityRole>>();

      identityBuilder.AddEntityFrameworkStores<CursosOnlineContext>();
      identityBuilder.AddSignInManager<SignInManager<Usuario>>();

      services.TryAddSingleton<ISystemClock, SystemClock>();

      // Configuración para los JWT Tokens
      var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("Mi palabra secreta"));
      services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(opt =>
      {
        opt.TokenValidationParameters = new TokenValidationParameters
        {
          ValidateIssuerSigningKey = true,
          IssuerSigningKey = key,
          ValidateAudience = false, // Alguien con una IP cualquiera pueda generar un Token
          ValidateIssuer = false, // Es para el envio del token (?)
        };
      });

      services.AddScoped<IJwtGenerador, JwtGenerador>();
      services.AddScoped<IUsuarioSesion, UsuarioSesion>();


      services.AddAutoMapper(typeof(Consulta.Manejador));

      services.AddTransient<IFactoryConnection, FactoryConnection>();
      services.AddScoped<IInstructor, InstructorRepositorio>();
      services.AddScoped<IPaginacion, PaginacionRepositorio>();

      // Configuración de Swagger
      services.AddSwaggerGen(c =>
      {
        c.SwaggerDoc("v1", new OpenApiInfo
        {
          Title = "Servicios para mantenimineto de cursos",
          Version = "v1"
        });

        c.CustomSchemaIds(c => c.FullName);
      });




    }

    // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {

      // Indico que la app use la policy "corsApp"definida en ConfigureServices.      
      app.UseCors("corsApp");

      app.UseMiddleware<ManejadorErrorMiddleware>();

      if (env.IsDevelopment())
      {
        //app.UseDeveloperExceptionPage();
      }

      // app.UseHttpsRedirection();

      // Con la siguiente linea declaro que mi aplicación va a utilizar la autenticación por Tokens
      app.UseAuthentication();

      app.UseRouting();

      app.UseAuthorization();

      app.UseEndpoints(endpoints =>
      {
        endpoints.MapControllers();
      });

      app.UseSwagger();
      app.UseSwaggerUI(c =>
      {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Cursos online v1");
      });
    }
  }
}