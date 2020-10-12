using System;
using System.Threading;
using System.Threading.Tasks;
using FluentValidation;
using MediatR;
using Persistencia.DapperConexion.Instructor;

namespace Aplicacion.Instructores
{
  public class Editar
  {
    public class Ejecuta : IRequest
    {
      public Guid InstructorId { get; set; }
      public string Nombre { get; set; }
      public string Apellidos { get; set; }
      public string Grado { get; set; }
    }

    public class EjecutaValida : AbstractValidator<Ejecuta>
    {
      public EjecutaValida()
      {
        RuleFor(ins => ins.Nombre).NotEmpty();
        RuleFor(ins => ins.Apellidos).NotEmpty();
        RuleFor(ins => ins.Grado).NotEmpty();
      }
    }

    public class Manejador : IRequestHandler<Ejecuta>
    {
      private readonly IInstructor instructorRepositorio;

      public Manejador(IInstructor instructorRepositorio)
      {
        this.instructorRepositorio = instructorRepositorio;
      }

      public async Task<Unit> Handle(Ejecuta request, CancellationToken cancellationToken)
      {
        try
        {
          var resultado = await this.instructorRepositorio.Actualiza(request.InstructorId, request.Nombre, request.Apellidos, request.Grado);

          if (resultado > 0)
          {
            return Unit.Value;
          }

          throw new Exception("Ocurrió un error al editar el instructor");
        }
        catch (System.Exception)
        {

          throw;
        }
      }
    }
  }
}