
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


      public string NombreCompleto { get; set; }
      public string Email { get; set; }
      public string Password { get; set; }
      public string Username { get; set; }
      public ImagenGeneral ImagenPerfil { get; set; }

    }
    public class EjecutaValidator : AbstractValidator<Ejecuta>
    {
      public EjecutaValidator()
      {
        RuleFor(u => u.NombreCompleto).NotEmpty();
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
        if (request.ImagenPerfil != null)
        {

          var userIdGuid = new Guid(user.Id);
          var resultadoImagen = await this.context.Documento.Where(d => d.ObjetoReferencia == userIdGuid).FirstOrDefaultAsync();

          if (resultadoImagen == null)
          {

            var imagen = new Documento
            {
              DocumentoId = Guid.NewGuid(),
              Contenido = Convert.FromBase64String(request.ImagenPerfil.Data),
              Nombre = request.ImagenPerfil.Nombre,
              Extension = request.ImagenPerfil.Extension,
              ObjetoReferencia = userIdGuid,
              FechaCreacion = DateTime.UtcNow
            };

            await this.context.Documento.AddAsync(imagen);
          }
          else
          {
            resultadoImagen.Contenido = Convert.FromBase64String(request.ImagenPerfil.Data);
            resultadoImagen.Nombre = request.ImagenPerfil.Nombre;
            resultadoImagen.Extension = request.ImagenPerfil.Extension;
            resultadoImagen.ObjetoReferencia = userIdGuid;
          }

          await this.context.SaveChangesAsync(); //???


        }

        user.NombreCompleto = request.NombreCompleto;
        user.PasswordHash = this.passwordHasher.HashPassword(user, request.Password);
        user.Email = request.Email;
        var resultado = await this.userManager.UpdateAsync(user);


        if (resultado.Succeeded)
        {
          var imagenPerfil = await this.context.Documento.Where(x => x.ObjetoReferencia == new Guid(user.Id)).FirstOrDefaultAsync();
          ImagenGeneral imagenUsuario = null;
          if (imagenPerfil != null)
          {
            imagenUsuario = new ImagenGeneral
            {
              Data = Convert.ToBase64String(imagenPerfil.Contenido),
              Nombre = imagenPerfil.Nombre,
              Extension = imagenPerfil.Extension
            };
          }

          var roles = await this.userManager.GetRolesAsync(user);

          return new UsuarioData
          {
            NombreCompleto = user.NombreCompleto,
            Email = user.Email,
            UserName = user.UserName,
            Token = this.jwtGenerador.CrearToken(user, new List<string>(roles)),
            ImagenPerfil = imagenUsuario
          };
        }


        throw new Exception("Ocurrio un error al actualizar el usuario");


      }
    }
  }
}

