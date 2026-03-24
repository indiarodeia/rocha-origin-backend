using Api.Models;
using Api.Models.Enum;
using Microsoft.EntityFrameworkCore;
using Route = Api.Models.Route;

namespace Api.Data;

public sealed class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {

    }

    // Models
    public DbSet<Address> Addresses => Set<Address>();
    public DbSet<Client> Clients => Set<Client>();
    public DbSet<Establishment> Establishments => Set<Establishment>();
    public DbSet<EstablishmentMenuItem> EstablishmentMenuItems => Set<EstablishmentMenuItem>();
    public DbSet<Route> Routes => Set<Route>();
    public DbSet<Product> Products => Set<Product>();

    //Enums
    public DbSet<PaymentType> PaymentTypes => Set<PaymentType>();
    public DbSet<ProductUnit> ProductUnits => Set<ProductUnit>();
    public DbSet<ProductCategory> ProductCategories => Set<ProductCategory>();


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<PaymentType>().HasData(
            new PaymentType
            {
                Id = 1,
                Label = "Immediate",
                Description = "Immediate payment",
                Order = 1,
                IsActive = true
            },
            new PaymentType
            {
                Id = 2,
                Label = "Credit",
                Description = "Credit payment",
                Order = 2,
                IsActive = true
            },
            new PaymentType
            {
                Id = 3,
                Label = "Customer",
                Description = "Customer-defined payment",
                Order = 3,
                IsActive = true
            }
        );

        modelBuilder.Entity<ProductUnit>().HasData(
            new ProductUnit
            {
                Id = 1,
                Label = "KG",
                Description = "Kilogram",
                Order = 1,
                IsActive = true
            },
            new ProductUnit
            {
                Id = 2,
                Label = "UN",
                Description = "Unit",
                Order = 2,
                IsActive = true
            }
        );

        modelBuilder.Entity<ProductCategory>().HasData(
            new ProductCategory
            {
                Id = (int)ProductCategoryEnum.Bovino,
                Label = "Bovino",
                Description = "Produtos da categoria bovino",
                Order = 1,
                IsActive = true
            },
            new ProductCategory
            {
                Id = (int)ProductCategoryEnum.Transformados,
                Label = "Transformados",
                Description = "Produtos transformados",
                Order = 2,
                IsActive = true
            }
        );
    }

}