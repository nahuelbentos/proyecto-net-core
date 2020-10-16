using System.Net;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistencia;
using Aplicacion.ManejadorError;

namespace Aplicacion.Documentos
{
  public class ObtenerArchivo
  {
    public class Ejecuta : IRequest<ArchivoGenerico>
    {
      public Guid DocumentoId { get; set; }
      public Guid? ObjetoReferencia { get; set; }

    }

    public class Manejador : IRequestHandler<Ejecuta, ArchivoGenerico>
    {
      private readonly CursosOnlineContext context;

      public Manejador(CursosOnlineContext context)
      {
        this.context = context;
      }

      public async Task<ArchivoGenerico> Handle(Ejecuta request, CancellationToken cancellationToken)
      {
        var archivo = await this.context.Documento.Where(d => d.DocumentoId == request.DocumentoId).FirstOrDefaultAsync();

        if (archivo == null)
        {
          throw new ManejadorExcepcion(HttpStatusCode.NotFound, new { mensaje = "El archivo no existe" });
        }

        return new ArchivoGenerico
        {
          Data = Convert.ToBase64String(archivo.Contenido),
          Nombre = archivo.Nombre,
          Extension = archivo.Extension,
        };

      }
    }
  }
}