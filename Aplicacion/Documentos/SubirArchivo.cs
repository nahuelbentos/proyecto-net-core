using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Dominio;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistencia;

namespace Aplicacion.Documentos
{
  public class SubirArchivo
  {
    public class Ejecuta : IRequest
    {
      public Guid? ObjetoReferencia { get; set; }
      public string Data { get; set; }
      public string Nombre { get; set; }
      public string Extension { get; set; }
    }
    public class EjecutaValidacion : AbstractValidator<Ejecuta>
    {
      public EjecutaValidacion()
      {
        RuleFor(d => d.Data).NotEmpty();
        RuleFor(d => d.Nombre).NotEmpty();
        RuleFor(d => d.Extension).NotEmpty();
      }
    }
    public class Manejador : IRequestHandler<Ejecuta>
    {
      private readonly CursosOnlineContext context;

      public Manejador(CursosOnlineContext context)
      {
        this.context = context;
      }

      public async Task<Unit> Handle(Ejecuta request, CancellationToken cancellationToken)
      {

        var documento = await this.context.Documento.Where(d => d.ObjetoReferencia == request.ObjetoReferencia).FirstOrDefaultAsync();
        if (documento == null)
        {
          var doc = new Documento
          {
            DocumentoId = Guid.NewGuid(),
            Contenido = Convert.FromBase64String(request.Data),
            Nombre = request.Nombre,
            Extension = request.Extension,
            ObjetoReferencia = request.ObjetoReferencia ?? Guid.Empty,
            FechaCreacion = DateTime.UtcNow,
          };

          this.context.Documento.Add(doc);
        }
        else
        {
          documento.Contenido = Convert.FromBase64String(request.Data);
          documento.Nombre = request.Nombre;
          documento.Extension = request.Extension;
          documento.ObjetoReferencia = request.ObjetoReferencia ?? Guid.Empty;
          documento.FechaCreacion = DateTime.UtcNow;
        }


        var resultado = await this.context.SaveChangesAsync();

        if (resultado > 0)
        {
          return Unit.Value;
        }

        throw new Exception("Ocurrio un error al guardar el archivo");
      }
    }
  }
}