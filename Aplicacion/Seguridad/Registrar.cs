using System.Net;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Aplicacion.Contratos;
using Aplicacion.ManejadorError;
using Dominio;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Persistencia;
using FluentValidation;

namespace Aplicacion.Seguridad
{
  public class Registrar
  {
    public class Ejecuta : IRequest<UsuarioData>
    {
      public string Nombre { get; set; }
      public string Apellidos { get; set; }

      public string Email { get; set; }

      public string Password { get; set; }

      public string UserName { get; set; }

    }

    public class EjecutaValidador : AbstractValidator<Ejecuta>
    {
      public EjecutaValidador()
      {
        RuleFor(u => u.Nombre).NotEmpty();
        RuleFor(u => u.Apellidos).NotEmpty();
        RuleFor(u => u.Email).NotEmpty();
        RuleFor(u => u.Password).NotEmpty();
        RuleFor(u => u.UserName).NotEmpty();
      }
    }

    public class Manejador : IRequestHandler<Ejecuta, UsuarioData>
    {
      private readonly CursosOnlineContext context;
      private readonly UserManager<Usuario> userManager;
      private readonly IJwtGenerador jwtGenerador;

      public Manejador(CursosOnlineContext context, UserManager<Usuario> userManager, IJwtGenerador jwtGenerador)
      {
        this.context = context;
        this.userManager = userManager;
        this.jwtGenerador = jwtGenerador;
      }

      public async Task<UsuarioData> Handle(Ejecuta request, CancellationToken cancellationToken)
      {
        var existe = await this.context.Users.Where(user => user.Email == request.Email).AnyAsync();

        if (existe)
        {
          throw new ManejadorExcepcion(HttpStatusCode.BadRequest, new { mensaje = "El email ingresado ya existe" });
        }

        var existeUserName = await this.context.Users.Where(user => user.UserName == request.UserName).AnyAsync();

        if (existeUserName)
        {
          throw new ManejadorExcepcion(HttpStatusCode.BadRequest, new { mensaje = "El UserName ingresado ya existe" });
        }


        var usuario = new Usuario
        {
          NombreCompleto = request.Nombre + "" + request.Apellidos,
          Email = request.Email,
          UserName = request.UserName
        };

        var resultado = await this.userManager.CreateAsync(usuario, request.Password);
        if (resultado.Succeeded)
          return new UsuarioData
          {
            NombreCompleto = usuario.NombreCompleto,
            Token = this.jwtGenerador.CrearToken(usuario),
            UserName = usuario.UserName,
            Email = usuario.Email
          };

        throw new ManejadorExcepcion(HttpStatusCode.InternalServerError, new { mensaje = "Ocurrió un error" });
      }
    }

  }
}