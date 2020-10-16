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
  public class Login
  {
    public class Ejecuta : IRequest<UsuarioData>
    {

      public string Email { get; set; }
      public string Password { get; set; }
    }

    public class EjecutaValidacion : AbstractValidator<Ejecuta>
    {
      public EjecutaValidacion()
      {
        RuleFor(x => x.Email).NotEmpty();
        RuleFor(x => x.Password).NotEmpty();
      }
    }
    public class Manejador : IRequestHandler<Ejecuta, UsuarioData>
    {
      private readonly UserManager<Usuario> userManager;
      private readonly SignInManager<Usuario> signInManager;
      private readonly IJwtGenerador jwtGenerador;
      private readonly CursosOnlineContext context;

      public Manejador(UserManager<Usuario> userManager, SignInManager<Usuario> signInManager, IJwtGenerador jwtGenerador, CursosOnlineContext context)
      {
        this.userManager = userManager;
        this.signInManager = signInManager;
        this.jwtGenerador = jwtGenerador;
        this.context = context;
      }

      public async Task<UsuarioData> Handle(Ejecuta request, CancellationToken cancellationToken)
      {
        var usuario = await this.userManager.FindByEmailAsync(request.Email);

        if (usuario == null)
        {
          throw new ManejadorExcepcion(HttpStatusCode.Unauthorized);
        }

        var resultado = await signInManager.CheckPasswordSignInAsync(usuario, request.Password, false);


        if (resultado.Succeeded)
        {

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
            NombreCompleto = usuario.NombreCompleto,
            Token = this.jwtGenerador.CrearToken(usuario, new List<string>(listaRoles)),
            Email = usuario.Email,
            UserName = usuario.UserName,
            ImagenPerfil = imagenUsuario,
          };
        }

        throw new ManejadorExcepcion(HttpStatusCode.Unauthorized);




      }
    }


  }
}