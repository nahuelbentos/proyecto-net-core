using System.Net.Mime;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Aplicacion.Contratos;
using Dominio;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Persistencia;

namespace Aplicacion.Seguridad
{
  public class UsuarioActual
  {
    public class Ejecutar : IRequest<UsuarioData> { }

    public class Manejador : IRequestHandler<Ejecutar, UsuarioData>
    {
      private readonly UserManager<Usuario> userManager;
      private readonly IJwtGenerador jwtGenerador;
      private readonly IUsuarioSesion usuarioSesion;
      private readonly CursosOnlineContext context;

      public Manejador(UserManager<Usuario> userManager, IJwtGenerador jwtGenerador, IUsuarioSesion usuarioSesion, CursosOnlineContext context)
      {
        this.userManager = userManager;
        this.jwtGenerador = jwtGenerador;
        this.usuarioSesion = usuarioSesion;
        this.context = context;
      }

      public async Task<UsuarioData> Handle(Ejecutar request, CancellationToken cancellationToken)
      {
        var usuario = await userManager.FindByNameAsync(usuarioSesion.ObtenerUsuarioSesion());

        var listaRoles = await this.userManager.GetRolesAsync(usuario);
        var roles = new List<string>(listaRoles);

        var imagen = await this.context.Documento.Where(d => d.ObjetoReferencia == new Guid(usuario.Id)).FirstOrDefaultAsync();

        ImagenGeneral imagenUsuario = null;
        if (imagen != null)
        {
          imagenUsuario = new ImagenGeneral
          {
            Data = Convert.ToBase64String(imagen.Contenido),
            Extension = imagen.Extension,
            Nombre = imagen.Nombre
          };
        }

        return new UsuarioData
        {
          Email = usuario.Email,
          NombreCompleto = usuario.NombreCompleto,
          UserName = usuario.UserName,
          Token = jwtGenerador.CrearToken(usuario, new List<string>(listaRoles)),
          ImagenPerfil = imagenUsuario
        };
      }
    }
  }
}