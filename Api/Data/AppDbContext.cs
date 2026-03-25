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
    public DbSet<Supplier> Suppliers => Set<Supplier>();
    public DbSet<Animal> Animals => Set<Animal>();
    public DbSet<Order> Orders => Set<Order>();
    public DbSet<OrderItem> OrderItems => Set<OrderItem>();
    public DbSet<Lot> Lots => Set<Lot>();
    public DbSet<LotAnimal> LotAnimals => Set<LotAnimal>();

    //Enums
    public DbSet<PaymentType> PaymentTypes => Set<PaymentType>();
    public DbSet<ProductUnit> ProductUnits => Set<ProductUnit>();
    public DbSet<ProductCategory> ProductCategories => Set<ProductCategory>();
    public DbSet<AnimalSpecies> AnimalSpecies => Set<AnimalSpecies>();
    public DbSet<OrderStatus> OrderStatuses => Set<OrderStatus>();
    public DbSet<DeliveryType> DeliveryTypes => Set<DeliveryType>();
    public DbSet<TraceabilitySourceType> TraceabilitySourceTypes => Set<TraceabilitySourceType>();


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

        modelBuilder.Entity<AnimalSpecies>().HasData(
            new AnimalSpecies
            {
                Id = (int)AnimalSpeciesEnum.Bovino,
                Label = "BOVINO",
                Description = "Espécie bovina",
                Order = 1,
                IsActive = true
            },
            new AnimalSpecies
            {
                Id = (int)AnimalSpeciesEnum.Ovino,
                Label = "OVINO",
                Description = "Espécie ovina",
                Order = 2,
                IsActive = true
            }
        );

        modelBuilder.Entity<OrderStatus>().HasData(
            new OrderStatus
            {
                Id = (int)OrderStatusEnum.Pending,
                Label = "PENDING",
                Description = "Order created and awaiting preparation",
                Order = 1,
                IsActive = true
            },
            new OrderStatus
            {
                Id = (int)OrderStatusEnum.Preparing,
                Label = "PREPARING",
                Description = "Order is being prepared",
                Order = 2,
                IsActive = true
            },
            new OrderStatus
            {
                Id = (int)OrderStatusEnum.Ready,
                Label = "READY",
                Description = "Order is ready for delivery or pickup",
                Order = 3,
                IsActive = true
            },
            new OrderStatus
            {
                Id = (int)OrderStatusEnum.Delivered,
                Label = "DELIVERED",
                Description = "Order has been delivered",
                Order = 4,
                IsActive = true
            }
        );

        modelBuilder.Entity<DeliveryType>().HasData(
            new DeliveryType
            {
                Id = (int)DeliveryTypeEnum.Delivery,
                Label = "DELIVERY",
                Description = "Delivered to the client",
                Order = 1,
                IsActive = true
            },
            new DeliveryType
            {
                Id = (int)DeliveryTypeEnum.Pickup,
                Label = "PICKUP",
                Description = "Picked up by the client",
                Order = 2,
                IsActive = true
            }
        );

        modelBuilder.Entity<TraceabilitySourceType>().HasData(
            new TraceabilitySourceType
            {
                Id = (int)TraceabilitySourceTypeEnum.None,
                Label = "NONE",
                Description = "No traceability source assigned",
                Order = 1,
                IsActive = true
            },
            new TraceabilitySourceType
            {
                Id = (int)TraceabilitySourceTypeEnum.Animal,
                Label = "ANIMAL",
                Description = "Traceability comes from an animal",
                Order = 2,
                IsActive = true
            },
            new TraceabilitySourceType
            {
                Id = (int)TraceabilitySourceTypeEnum.Lot,
                Label = "LOT",
                Description = "Traceability comes from a lot",
                Order = 3,
                IsActive = true
            }
        );
    }

}