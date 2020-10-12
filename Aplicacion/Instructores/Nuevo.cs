using System;
using System.Threading;
using System.Threading.Tasks;
using FluentValidation;
using MediatR;
using Persistencia.DapperConexion.Instructor;

namespace Aplicacion.Instructores
{
  public class Nuevo
  {
    public class Ejecuta : IRequest
    {
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


      public class Manejador : IRequestHandler<Ejecuta>
      {
        private readonly IInstructor instructorRepository;

        public Manejador(IInstructor instructorRepository)
        {
          this.instructorRepository = instructorRepository;
        }

        public async Task<Unit> Handle(Ejecuta request, CancellationToken cancellationToken)
        {

          var resultado = await this.instructorRepository.Nuevo(request.Nombre, request.Apellidos, request.Grado);

          if (resultado > 0)
          {
            return Unit.Value;
          }

          throw new Exception("No se pudo insertar el instructor");

        }
      }
    }
  }
}