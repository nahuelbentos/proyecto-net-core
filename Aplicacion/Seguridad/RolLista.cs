using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Aplicacion.ManejadorError;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Persistencia;

namespace Aplicacion.Seguridad
{
  public class RolLista
  {
    public class Ejecuta : IRequest<List<IdentityRole>>
    {

    }

    public class Manejador : IRequestHandler<Ejecuta, List<IdentityRole>>
    {
      private readonly RoleManager<IdentityRole> roleManager;
      private readonly CursosOnlineContext context;

      public Manejador(RoleManager<IdentityRole> roleManager, CursosOnlineContext context)
      {
        this.roleManager = roleManager;
        this.context = context;
      }

      public async Task<List<IdentityRole>> Handle(Ejecuta request, CancellationToken cancellationToken)
      {
        var roles = await this.context.Roles.ToListAsync();

        return roles;

      }
    }
  }
}