using System.Net;
using System;
using System.Threading;
using System.Threading.Tasks;
using Aplicacion.ManejadorError;
using Dominio;
using MediatR;
using Persistencia;

namespace Aplicacion.Comentarios
{
  public class Eliminar
  {
    public class Ejecuta : IRequest
    {
      public Guid Id { get; set; }
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
        var comentario = await this.context.Comentario.FindAsync(request.Id);

        if (comentario == null)
        {
          throw new ManejadorExcepcion(HttpStatusCode.NotFound, new { mensaje = "No se encontro el comenatrio" });
        }

        this.context.Comentario.Remove(comentario);

        var resultado = await this.context.SaveChangesAsync();

        if (resultado > 0)
        {
          return Unit.Value;
        }

        throw new Exception("Ocurrio un error al eliminar el comentario");

      }
    }
  }
}