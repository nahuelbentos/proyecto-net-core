using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Persistencia;

namespace Aplicacion.Cursos
{
  public class Eliminar
  {
    public class Ejecuta : IRequest
    {
      public int Id { get; set; }
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
        var curso = await this.context.Curso.FindAsync(request.Id);

        if (curso == null)
          throw new Exception("No existe el curso con el id: " + request.Id);

        this.context.Remove(curso);

        var valor = await this.context.SaveChangesAsync();

        if (valor > 0)
          return Unit.Value;

        throw new Exception("No se pudo eliminar el curso.");


      }
    }
  }
}