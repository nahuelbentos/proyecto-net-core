using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Aplicacion.Contratos;
using Aplicacion.ManejadorError;
using Dominio;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Persistencia;

namespace Aplicacion.Seguridad
{
  public class UsuarioActualizar
  {
    public class Ejecuta : IRequest<UsuarioData>
    {


      public string Nombre { get; set; }
      public string Apellidos { get; set; }
      public string Email { get; set; }
      public string Password { get; set; }
      public string Username { get; set; }

    }
    public class EjecutaValidator : AbstractValidator<Ejecuta>
    {
      public EjecutaValidator()
      {
        RuleFor(u => u.Nombre).NotEmpty();
        RuleFor(u => u.Apellidos).NotEmpty();
        RuleFor(u => u.Email).NotEmpty();
        RuleFor(u => u.Password).NotEmpty();
        RuleFor(u => u.Username).NotEmpty();
      }
    }

    public class Manejador : IRequestHandler<Ejecuta, UsuarioData>
    {
      private readonly UserManager<Usuario> userManager;
      private readonly CursosOnlineContext context;
      private readonly IJwtGenerador jwtGenerador;
      private readonly IPasswordHasher<Usuario> passwordHasher;

      public Manejador(UserManager<Usuario> userManager, CursosOnlineContext context, IJwtGenerador jwtGenerador, IPasswordHasher<Usuario> passwordHasher)
      {
        this.userManager = userManager;
        this.context = context;
        this.jwtGenerador = jwtGenerador;
        this.passwordHasher = passwordHasher;
      }

      public async Task<UsuarioData> Handle(Ejecuta request, CancellationToken cancellationToken)
      {
        var user = await this.userManager.FindByNameAsync(request.Username);

        if (user == null)
        {
          throw new ManejadorExcepcion(HttpStatusCode.NotFound, new { mensaje = "No existe el usuario ingresado." });
        }

        var existeOtroUsuario = await this.context.Users.Where(u => u.Email == request.Email && u.UserName != request.Username).AnyAsync();

        if (existeOtroUsuario)
        {
          throw new ManejadorExcepcion(HttpStatusCode.InternalServerError, new { mensaje = "Ya existe ese mail en otro usuario" });
        }

        user.NombreCompleto = request.Nombre + " " + request.Apellidos;
        user.PasswordHash = this.passwordHasher.HashPassword(user, request.Password);
        user.Email = request.Email;

        var resultado = await this.userManager.UpdateAsync(user);


        if (resultado.Succeeded)
        {

          var roles = await this.userManager.GetRolesAsync(user);

          return new UsuarioData
          {
            NombreCompleto = user.NombreCompleto,
            Email = user.Email,
            UserName = user.UserName,
            Token = this.jwtGenerador.CrearToken(user, new List<string>(roles))
          };
        }


        throw new Exception("Ocurrio un error al actualizar el usuario");


      }
    }
  }
}

