using back_end.Entidades;
using Microsoft.EntityFrameworkCore;

namespace back_end {

    public class ApplicationDbContext : DbContext {

        public ApplicationDbContext(DbContextOptions options) : base(options) { }

        public DbSet<Actor> Actores { get; set; }
        public DbSet<Cine> Cines { get; set; }
        public DbSet<Genero> Generos { get; set; }
        public DbSet<Pelicula> Peliculas { get; set; }
        public DbSet<PeliculaActor> PeliculasActores { get; set; }
        public DbSet<PeliculaCine> PeliculasCines { get; set; }
        public DbSet<PeliculaGenero> PeliculasGeneros { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder) {
            modelBuilder.Entity<PeliculaActor>().HasKey(x => new { x.IdPelicula, x.IdActor });
            modelBuilder.Entity<PeliculaCine>().HasKey(x => new { x.IdPelicula, x.IdCine });
            modelBuilder.Entity<PeliculaGenero>().HasKey(x => new { x.IdPelicula, x.IdGenero });

            base.OnModelCreating(modelBuilder);
        }

    }

}