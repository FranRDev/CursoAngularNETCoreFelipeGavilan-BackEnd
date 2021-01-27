using back_end.DTOs;
using back_end.Entidades;
using Microsoft.EntityFrameworkCore;

namespace back_end {

    public class ApplicationDbContext : DbContext {

        public ApplicationDbContext(DbContextOptions options) : base(options) { }

        public DbSet<Actor> Actores { get; set; }
        public DbSet<Cine> Cines { get; set; }
        public DbSet<Genero> Generos { get; set; }

    }

}