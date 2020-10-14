using System.Collections.Generic;
using System.Threading.Tasks;
using Aplicacion.Seguridad;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers
{
  public class RolController : MiControllerBase
  {
    [HttpPost("crear")]
    public async Task<ActionResult<Unit>> Crear(RolNuevo.Ejecuta data)
    {
      return await Mediator.Send(data);
    }

    [HttpDelete("eliminar")]
    public async Task<ActionResult<Unit>> Eliminar(RolEliminar.Ejecuta data)
    {
      return await Mediator.Send(data);
    }


    [HttpGet("lista")]
    public async Task<ActionResult<List<IdentityRole>>> Lista()
    {
      return await Mediator.Send(new RolLista.Ejecuta());
    }

    [HttpPost("agregarRoleUsuario")]
    public async Task<ActionResult<Unit>> AgregarRoleUsuario(UsuarioRolAgregar.Ejecuta data)
    {
      return await Mediator.Send(data);
    }


    [HttpDelete("eliminarRoleUsuario")]
    public async Task<ActionResult<Unit>> Eliminar(UsuarioRolEliminar.Ejecuta data)
    {
      return await Mediator.Send(data);
    }



    [HttpGet("obtenerPorUsuario/{username}")]
    public async Task<ActionResult<List<string>>> obtenerPorUsuario(string username)
    {
      return await Mediator.Send(new ObtenerRolesPorUsuario.Ejecuta { Username = username });
    }


  }
}