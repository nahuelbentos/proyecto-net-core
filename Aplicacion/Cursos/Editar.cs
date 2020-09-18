
using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Aplicacion.ManejadorError;
using FluentValidation;
using MediatR;
using Persistencia;

namespace Aplicacion.Cursos
{
  public class Editar
  {
    public class Ejecuta : IRequest
    {

      public int CursoId { get; set; }
      public string Titulo { get; set; }

      public string Descripcion { get; set; }

      // Agregando el '?' dejo que el campo DateTime perimta nulos
      public DateTime? FechaPublicacion { get; set; }
    }


    public class EjecutaValidacion : AbstractValidator<Ejecuta>
    {
      public EjecutaValidacion()
      {
        RuleFor(x => x.Titulo).NotEmpty().WithMessage("El titulo es requerido papu");
        RuleFor(x => x.Descripcion).NotEmpty();
        RuleFor(x => x.FechaPublicacion).NotEmpty();
      }
    }

    public class Manejador : IRequestHandler<Ejecuta>
    {
      private readonly CursosOnlineContext _context;

      public Manejador(CursosOnlineContext context)
      {
        this._context = context;
      }

      public async Task<Unit> Handle(Ejecuta request, CancellationToken cancellationToken)
      {
        var curso = await this._context.Curso.FindAsync(request.CursoId);

        if (curso == null)
        {

          throw new ManejadorExcepcion(HttpStatusCode.NotFound, new { mensaje = "El curso no existe" });

        }

        curso.Titulo = request.Titulo ?? curso.Titulo;
        curso.Descripcion = request.Descripcion ?? curso.Descripcion;
        curso.FechaPublicacion = request.FechaPublicacion ?? curso.FechaPublicacion;

        var valor = await _context.SaveChangesAsync();


        if (valor > 0)
          return Unit.Value;

        throw new Exception("No se pudo editar el curso.");


      }
    }
  }
}