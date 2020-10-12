using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Aplicacion.ManejadorError;
using MediatR;
using Persistencia.DapperConexion.Instructor;

namespace Aplicacion.Instructores
{
  public class ConsultaById
  {
    public class Ejecuta : IRequest<InstructorModel>
    {
      public Guid Id { get; set; }
    }

    public class Manejador : IRequestHandler<Ejecuta, InstructorModel>
    {
      private readonly IInstructor instructorRepository;

      public Manejador(IInstructor instructorRepository)
      {
        this.instructorRepository = instructorRepository;
      }

      public async Task<InstructorModel> Handle(Ejecuta request, CancellationToken cancellationToken)
      {
        var instructor = await this.instructorRepository.ObtenerPorId(request.Id);

        if (instructor == null)
        {
          throw new ManejadorExcepcion(HttpStatusCode.NotFound, new { mensaje = "no es encontro el instructor" });
        }

        return instructor;
      }
    }
  }
}