using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Aplicacion.ManejadorError;
using AutoMapper;
using Dominio;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistencia;

namespace Aplicacion.Cursos
{
  public class ConsultaId
  {
    public class CursoUnico : IRequest<CursoDTO>
    {
      public Guid Id { get; set; }
    }

    public class Manejador : IRequestHandler<CursoUnico, CursoDTO>
    {
      private readonly CursosOnlineContext _context;
      private readonly IMapper mapper;

      public Manejador(CursosOnlineContext context, IMapper mapper)
      {
        this._context = context;
        this.mapper = mapper;
      }

      public async Task<CursoDTO> Handle(CursoUnico request, CancellationToken cancellationToken)
      {
        var curso = await this._context.Curso
                                    .Include(curso => curso.PrecioPromocion)
                                    .Include(curso => curso.ComentarioLista)
                                    .Include(curso => curso.InstructorLink)
                                    .ThenInclude(cursoInstructor => cursoInstructor.Instructor)
                                      .FirstOrDefaultAsync(c => c.CursoId == request.Id);

        if (curso == null)
        {

          throw new ManejadorExcepcion(HttpStatusCode.NotFound, new { mensaje = "El curso no existe" });

        }

        var cursoDTO = this.mapper.Map<Curso, CursoDTO>(curso);

        return cursoDTO;
      }
    }
  }
}