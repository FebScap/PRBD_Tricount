using Azure;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using PRBD_Framework;
using System.Configuration;
using System.Data;

namespace prbd_2324_a01.Model;

public class PridContext : DbContextBase
{
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) {
        base.OnConfiguring(optionsBuilder);

        /*
         * SQLite
         */

        // var connectionString = ConfigurationManager.ConnectionStrings["SqliteConnectionString"].ConnectionString;
        // optionsBuilder.UseSqlite(connectionString);

        /*
         * SQL Server
         */

        var connectionString = ConfigurationManager.ConnectionStrings["MsSqlConnectionString"].ConnectionString;
        optionsBuilder.UseSqlServer(connectionString);

        ConfigureOptions(optionsBuilder);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder) {
        base.OnModelCreating(modelBuilder);

        SeedData(modelBuilder);
    }

    private static void ConfigureOptions(DbContextOptionsBuilder optionsBuilder) {
        optionsBuilder.UseLazyLoadingProxies()
            .LogTo(Console.WriteLine, LogLevel.Information) // permet de visualiser les requêtes SQL générées par LINQ
            .EnableSensitiveDataLogging()
            .EnableDetailedErrors() // attention : ralentit les requêtes
            ;
    }

    private void SeedData(ModelBuilder modelBuilder) {
        
    }

    
    // Création des tables
    public DbSet<User> Users => Set<User>();
    public DbSet<Tricount> Tricounts => Set<Tricount>();
    public DbSet<Subscription> Subscriptions => Set<Subscription>();
    public DbSet<Operation> Operations => Set<Operation>();
    public DbSet<Repartition> Repartitions => Set<Repartition>();
    public DbSet<Template> Templates => Set<Template>();
    public DbSet<TemplateItem> UTemplateItems => Set<TemplateItem>();
}