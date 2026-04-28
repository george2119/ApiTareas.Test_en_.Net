using Microsoft.EntityFrameworkCore;
using ApiTareas.Models;

namespace ApiTareas.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    //public DbSet<Usuario> Usuarios { get; set; } //usuarios para login
    // Esta es la línea clave que falta o está mal nombrada
    public DbSet<Tarea> Tareas { get; set; } 

    public DbSet<Usuario> Usuarios { get; set; }
    
}