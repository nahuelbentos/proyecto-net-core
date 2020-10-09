using System.Linq;
using Aplicacion.Cursos;
using AutoMapper;
using Dominio;

namespace Aplicacion
{
  public class MappingProfile : Profile
  {
    public MappingProfile()
    {
      CreateMap<Curso, CursoDTO>()
            .ForMember(curso => curso.Instructores, x => x.MapFrom(y => y.InstructorLink.Select(z => z.Instructor).ToList()))
            .ForMember(curso => curso.Comentarios, x => x.MapFrom(y => y.ComentarioLista))
            .ForMember(curso => curso.Precio, x => x.MapFrom(y => y.PrecioPromocion));
      CreateMap<CursoInstructor, CursoInstructorDTO>();
      CreateMap<Instructor, InstructorDTO>();
      CreateMap<Comentario, ComentarioDTO>();
      CreateMap<Precio, PrecioDTO>();
    }
  }
}