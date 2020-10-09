using System.Linq;
using System.Collections.Generic;

using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Aplicacion.ManejadorError;
using FluentValidation;
using MediatR;
using Persistencia;
using Dominio;

namespace Aplicacion.Cursos
{
  public class Editar
  {
    public class Ejecuta : IRequest
    {

      public Guid CursoId { get; set; }
      public string Titulo { get; set; }

      public string Descripcion { get; set; }

      // Agregando el '?' dejo que el campo DateTime perimta nulos
      public DateTime? FechaPublicacion { get; set; }

      public List<Guid> ListaInstructor { get; set; }

      public decimal? Precio { get; set; }
      public decimal? Promocion { get; set; }
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


        /* Actualizar el precio del curso */
        var precioEntidad = this._context.Precio.Where(x => x.CursoId == curso.CursoId).FirstOrDefault();

        if (precioEntidad != null)
        {
          precioEntidad.Promocion = request.Promocion ?? precioEntidad.Promocion;
          precioEntidad.PrecioActual = request.Precio ?? precioEntidad.PrecioActual;
        }
        else
        {
          precioEntidad = new Precio
          {
            PrecioId = Guid.NewGuid(),
            Promocion = request.Promocion ?? 0,
            PrecioActual = request.Precio ?? 0,
            CursoId = curso.CursoId
          };
          await this._context.Precio.AddAsync(precioEntidad);
        }
        /* Fin  Actualizar el precio del curso */

        if (request.ListaInstructor != null)
        {
          if (request.ListaInstructor.Count > 0)
          {
            /* Elimina los instructores actuales del curso en la base de datos */
            var instructoresBD = _context.CursoInstructor.Where(c => c.CursoId == request.CursoId).ToList();

            foreach (var instructorEliminar in instructoresBD)
            {
              this._context.CursoInstructor.Remove(instructorEliminar);
            }
            /* Fin Eliminar los instructores actuales del curso en la base de datos */

            /* Agregar los instructores nuevos al curso en la base de datos */
            foreach (var instructorAgregar in request.ListaInstructor)
            {
              var nuevoInstructor = new CursoInstructor
              {
                CursoId = request.CursoId,
                InstructorId = instructorAgregar
              };
              this._context.CursoInstructor.Add(nuevoInstructor);
            }

            /* Fin Agregar los instructores nuevos al curso en la base de datos */

          }
        }

        var valor = await _context.SaveChangesAsync();


        if (valor > 0)
          return Unit.Value;

        throw new Exception("No se pudo editar el curso.");


      }
    }
  }
}