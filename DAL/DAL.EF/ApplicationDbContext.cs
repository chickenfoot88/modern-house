using System.Data.Entity;
using DAL.EF.Migrations;
using Domain.Core.Positions.Entities;
using Domain.Dictionary.Cars.Entities;
using Domain.Dictionary.Containers.Entities;
using Domain.Dictionary.Customers.Entities;
using Domain.Dictionary.Drivers.Entities;
using Domain.Dictionary.Polygons.Entities;
using Domain.Registries.Requests.Entities;
using FileManager.Core.Entities;
using Domain.Dictionary.ContainerTypes.Entities;
using Domain.Registries.Schedules.Entities;
using Domain.Registries.Waybills.Entities;

namespace DAL.EF
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext() : base("default")
        {
            Database.SetInitializer(new MigrateDatabaseToLatestVersion<ApplicationDbContext, Configuration>(true));
            this.Configuration.LazyLoadingEnabled = true;
        }

        public DbSet<AttachedFileInfo> FileInfos { get; set; }

        //Служебные сущности
        public DbSet<Position> Positions { get; set; }

        //Справочники
        public DbSet<Car> Cars { get; set; }
        public DbSet<CarContainerType> CarContainerTypes { get; set; }
        public DbSet<Container> Containers { get; set; }
        public DbSet<ContainerType> ContainerTypes { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Driver> Drivers { get; set; }
        public DbSet<Polygon> Polygons { get; set; }

        //Реестры
        public DbSet<Request> Requests { get; set; }
        public DbSet<Schedule> Schedules { get; set; }
        public DbSet<Waybill> Waybills { get; set; }

        public DbSet<WaybillRequest> WaybillRequest { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
}
