using Azure;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations;
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
        modelBuilder.Entity<User>().HasData(
                new User("boverhaegen@epfc.eu", "3D4AEC0A9B43782133B8120B2FDD8C6104ABB513FE0CDCD0D1D4D791AA42E338:C217604FDAEA7291C7BA5D1D525815E4:100000:SHA256", "Boris", 0),
                new User("bepenelle@epfc.eu", "9E58D87797C6795D294E6762B6C05116D075BC18445AD4078C25674809DB57EF:C91E0B85B7264877C0424D52494D6296:100000:SHA256", "Boris", 0),
                new User("xapigeolet@epfc.eu", "5B979AB86EC73B0996F439D0BC3947ECCFA0A41310C77533EA36CB409DBB1243:0CF43009110DE4B4AA6D4E749F622755:100000:SHA256", "Boris", 0),
                new User("mamichel@epfc.eu", "955F147CE3473774E35EE58F4233AA84AE9118C6ECD4699DD788B8D588238034:5514D1DD0A97E9BA7FE4C0B5A4E89351:100000:SHA256", "Boris", 0),
                new User("admin@epfc.eu", "C9949A02A5DFBE50F1DA289DC162E3C97443AB09CE6F6EB1FD0C9D51B5241BBD:5533437973C5BC6459DB687CA5BDE76C:100000:SHA256", "Boris", 0)
            );

        int count = 0;
        modelBuilder.Entity<Operation>().HasData(
            new Operation { Id = ++count, Title = "Colruyt", Tricount = 4, Amount = 100, OperationDate = DateTime.Parse("2023/10/13"), Initiator = 2 },
            new Operation { Id = ++count, Title = "Plein essence", Tricount = 4, Amount = 75, OperationDate = DateTime.Parse("2023/10/13"), Initiator = 1 },
            new Operation { Id = ++count, Title = "Grosses courses LIDL", Tricount = 4, Amount = 212,74, OperationDate = DateTime.Parse("2023/10/13"), Initiator = 3 },
            new Operation { Id = ++count, Title = "Apéros", Tricount = 4, Amount = 31, 89745622, OperationDate = DateTime.Parse("2023/10/13"), Initiator = 1 },
            new Operation { Id = ++count, Title = "Boucherie", Tricount = 4, Amount = 25,5, OperationDate = DateTime.Parse("2023/10/26"), Initiator = 2 },
            new Operation { Id = ++count, Title = "Loterie", Tricount = 4, Amount = 35, OperationDate = DateTime.Parse("2023/10/26"), Initiator = 1 },
            new Operation { Id = ++count, Title = "Sangria", Tricount = 5, Amount = 42, OperationDate = DateTime.Parse("2023/10/16"), Initiator = 2 },
            new Operation { Id = ++count, Title = "Jet Ski", Tricount = 5, Amount = 250, OperationDate = DateTime.Parse("2023/08/17"), Initiator = 3 },
            new Operation { Id = ++count, Title = "PV Parking", Tricount = 5, Amount = 15,5, OperationDate = DateTime.Parse("2023/08/16"), Initiator = 3 },
            new Operation { Id = ++count, Title = "Tickets", Tricount = 6, Amount = 220, OperationDate = DateTime.Parse("2023/06/08"), Initiator = 1 },
            new Operation { Id = ++count, Title = "Décathlon", Tricount = 6, Amount = 199,99, OperationDate = DateTime.Parse("2023/07/01"), Initiator = 2 },
            )
        
    }

  

    
    // Création des tables
    public DbSet<User> Users => Set<User>();
    public DbSet<Tricount> Tricounts => Set<Tricount>();
    public DbSet<Subscription> Subscriptions => Set<Subscription>();
    public DbSet<Operation> Operations => Set<Operation>();

    public DbSet<Repartition> Repartitions => Set<Repartition>();
}