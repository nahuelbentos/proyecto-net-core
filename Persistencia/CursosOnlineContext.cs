using Dominio;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Persistencia
{
  public class CursosOnlineContext : IdentityDbContext<Usuario>
  {
    public CursosOnlineContext(DbContextOptions options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
      base.OnModelCreating(modelBuilder);
      modelBuilder.Entity<CursoInstructor>().HasKey(cursoInstructor => new { cursoInstructor.CursoId, cursoInstructor.InstructorId });
    }

    public DbSet<Curso> Curso { get; set; }
    public DbSet<Precio> Precio { get; set; }
    public DbSet<Comentario> Comentario { get; set; }
    public DbSet<Instructor> Instructor { get; set; }
    public DbSet<CursoInstructor> CursoInstructor { get; set; }

  }
}