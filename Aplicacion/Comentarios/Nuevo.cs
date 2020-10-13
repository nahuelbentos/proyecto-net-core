using System;
using System.Threading;
using System.Threading.Tasks;
using Dominio;
using FluentValidation;
using MediatR;
using Persistencia;

namespace Aplicacion.Comentarios
{
  public class Nuevo
  {
    public class Ejecuta : IRequest
    {
      public string Alumno { get; set; }

      public int Puntaje { get; set; }

      public string Comentario { get; set; }

      public Guid CursoId { get; set; }
    }

    public class EjecutaValidacion : AbstractValidator<Ejecuta>
    {
      public EjecutaValidacion()
      {
        RuleFor(comentario => comentario.Alumno).NotEmpty();
        RuleFor(comentario => comentario.Puntaje).NotEmpty();
        RuleFor(comentario => comentario.Comentario).NotEmpty();
        RuleFor(comentario => comentario.CursoId).NotEmpty();
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
        var comentario = new Comentario
        {
          ComentarioId = Guid.NewGuid(),
          Alumno = request.Alumno,
          ComentarioTexto = request.Comentario,
          Puntaje = request.Puntaje,
          CursoId = request.CursoId,
          FechaCreacion = DateTime.UtcNow
        };

        this.context.Comentario.Add(comentario);

        var resultado = await this.context.SaveChangesAsync();
        if (resultado > 0)
        {
          return Unit.Value;
        }
        throw new Exception("Ocurrio un error al crear el comentario, ");


      }
    }
  }
}