using System.Security.Claims;
using System.Linq;
using Aplicacion.Contratos;
using Microsoft.AspNetCore.Http;

namespace Seguridad
{
  public class UsuarioSesion : IUsuarioSesion
  {
    private readonly IHttpContextAccessor httpContextAccessor;

    public UsuarioSesion(IHttpContextAccessor httpContextAccessor)
    {
      this.httpContextAccessor = httpContextAccessor;
    }

    public string ObtenerUsuarioSesion()
    {
      var userName = this.httpContextAccessor.HttpContext.User?.Claims?.FirstOrDefault(claim => claim.Type == ClaimTypes.NameIdentifier)?.Value;
      return userName;
    }
  }
}