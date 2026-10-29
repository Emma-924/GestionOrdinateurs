using Microsoft.EntityFrameworkCore;
using GestionOrdinateurs.Shared;

namespace GestionOrdinateurs.API;

// Contexte EF Core de la base de données du parc informatique.
public class ParcInformatiqueContext : DbContext
{
    public DbSet<Ordinateur> Ordinateurs => Set<Ordinateur>();
    public DbSet<Reparation> Reparations => Set<Reparation>();
    public DbSet<User> Users => Set<User>();

    // Crée le contexte de la base de données du parc informatique.
    public ParcInformatiqueContext()
    {
    }

    // Crée le contexte à partir des options fournies (utilisé par les tests).
    public ParcInformatiqueContext(DbContextOptions<ParcInformatiqueContext> options) : base(options)
    {
    }

    // Configure la base de données utilisée par le contexte.
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            optionsBuilder.UseSqlite("Data Source=parcinfo.db");
        }
    }

    // Configure les caractéristiques de l'ordinateur, sa relation avec les réparations et l'unicité du login.
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Ordinateur>()
            .OwnsOne(o => o.Caracteristiques);

        modelBuilder.Entity<Ordinateur>()
            .HasMany(o => o.Reparations)
            .WithOne(r => r.UnOrdinateur);

        modelBuilder.Entity<User>()
            .HasIndex(u => u.Login)
            .IsUnique();
    }
}
