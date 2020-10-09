using System;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Aplicacion.ManejadorError;
using MediatR;
using Persistencia;

namespace Aplicacion.Cursos
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
        var instructoresBD = this.context.CursoInstructor.Where(c => c.CursoId == request.Id);

        foreach (var instructor in instructoresBD)
        {
          this.context.CursoInstructor.Remove(instructor);
        }

        var comentariosBD = this.context.Comentario.Where(c => c.CursoId == request.Id);

        foreach (var comentario in comentariosBD)
        {
          this.context.Comentario.Remove(comentario);
        }

        var precioBD = this.context.Precio.Where(c => c.CursoId == request.Id).FirstOrDefault();

        if (precioBD != null)
        {
          this.context.Precio.Remove(precioBD);
        }

        var curso = await this.context.Curso.FindAsync(request.Id);

        if (curso == null)
        {
          throw new ManejadorExcepcion(HttpStatusCode.NotFound, new { mensaje = "No se encontro el curso con el id: " + request.Id });
        }

        this.context.Remove(curso);

        var valor = await this.context.SaveChangesAsync();

        if (valor > 0)
          return Unit.Value;

        throw new Exception("No se pudo eliminar el curso.");


      }
    }
  }
}