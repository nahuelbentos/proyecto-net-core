using System.Net;
using System.Threading.Tasks;
using Aplicacion.Seguridad;
using Dominio;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers
{
  public class UsuarioController : MiControllerBase
  {

    //http://localhost:5000/api/usuario/login
    [HttpPost("login")]
    public async Task<ActionResult<UsuarioData>> Login(Login.Ejecuta data)
    {
      return await Mediator.Send(data);
    }
  }
}