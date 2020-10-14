using System.Net;
using System.Threading.Tasks;
using Aplicacion.Seguridad;
using Dominio;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers
{
  [AllowAnonymous]
  public class UsuarioController : MiControllerBase
  {

    //http://localhost:5000/api/usuario/login
    [HttpPost("login")]

    public async Task<ActionResult<UsuarioData>> Login(Login.Ejecuta data)
    {
      return await Mediator.Send(data);
    }


    //http://localhost:5000/api/usuario/registrar
    [HttpPost("registrar")]

    public async Task<ActionResult<UsuarioData>> Registrar(Registrar.Ejecuta data)
    {
      return await Mediator.Send(data);
    }

    //http://localhost:5000/api/usuario/
    [HttpGet]

    public async Task<ActionResult<UsuarioData>> ObtenerUsuarioActual()
    {
      return await Mediator.Send(new UsuarioActual.Ejecutar());
    }

    [HttpPut]
    public async Task<ActionResult<UsuarioData>> Actualizar(UsuarioActualizar.Ejecuta data)
    {
      return await Mediator.Send(data);
    }

  }
}