using System.Net;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Aplicacion.ManejadorError;
using Dominio;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace Aplicacion.Seguridad
{
  public class ObtenerRolesPorUsuario
  {
    public class Ejecuta : IRequest<List<string>>
    {
      public string Username { get; set; }
    }

    public class Manejador : IRequestHandler<Ejecuta, List<string>>
    {
      private readonly UserManager<Usuario> userManager;
      private readonly RoleManager<IdentityRole> roleManager;

      public Manejador(UserManager<Usuario> userManager, RoleManager<IdentityRole> roleManager)
      {
        this.userManager = userManager;
        this.roleManager = roleManager;
      }

      public async Task<List<string>> Handle(Ejecuta request, CancellationToken cancellationToken)
      {
        var user = await this.userManager.FindByNameAsync(request.Username);

        if (user == null)
        {
          throw new ManejadorExcepcion(HttpStatusCode.NotFound, new { mensaje = "No existe el usuario ingresado" });
        }

        var roles = await this.userManager.GetRolesAsync(user);

        return new List<string>(roles);
      }
    }
  }
}