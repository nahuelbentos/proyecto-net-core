using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Aplicacion.ManejadorError;
using Dominio;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace Aplicacion.Seguridad
{
  public class UsuarioRolEliminar
  {
    public class Ejecuta : IRequest
    {

      public string Username { get; set; }
      public string RolNombre { get; set; }

    }

    public class EjecutaValidador : AbstractValidator<Ejecuta>
    {
      public EjecutaValidador()
      {
        RuleFor(x => x.Username).NotEmpty();
        RuleFor(x => x.RolNombre).NotEmpty();
      }
    }

    public class Manejador : IRequestHandler<Ejecuta>
    {
      private readonly UserManager<Usuario> userManager;
      private readonly RoleManager<IdentityRole> roleManager;

      public Manejador(UserManager<Usuario> userManager, RoleManager<IdentityRole> roleManager)
      {
        this.userManager = userManager;
        this.roleManager = roleManager;
      }

      public async Task<Unit> Handle(Ejecuta request, CancellationToken cancellationToken)
      {
        var role = await this.roleManager.FindByNameAsync(request.RolNombre);
        if (role == null)
        {
          throw new ManejadorExcepcion(HttpStatusCode.NotFound, new { mensaje = "No existe el role ingresado" });
        }

        var user = await this.userManager.FindByNameAsync(request.Username);
        if (user == null)
        {
          throw new ManejadorExcepcion(HttpStatusCode.NotFound, new { mensaje = "No existe el usuario ingresado" });
        }

        var isInRole = await this.userManager.IsInRoleAsync(user, role.Name);
        if (!isInRole)
        {
          throw new ManejadorExcepcion(HttpStatusCode.BadRequest, new { mensaje = "El usuario no tiene el rol ingresado." });
        }

        var resultado = await this.userManager.RemoveFromRoleAsync(user, role.Name);

        if (resultado.Succeeded)
        {
          return Unit.Value;
        }

        throw new Exception("No se pudo eliminar el rol");


      }
    }
  }
}