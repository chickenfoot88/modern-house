using System.Data.Entity.Migrations;
using System.Diagnostics;
using System.Linq;
using Core;
using Core.DataAccess.Interfaces;
using Core.Enums;
using Core.Identity.Entities;
using Core.Identity.Services;
using DAL.EF.Implementations;
using Domain.Dictionary.Cars.Services;
using Domain.Dictionary.Containers.Services;
using Domain.Dictionary.ContainerTypes.Services;
using Domain.Dictionary.Customers.Services;
using Domain.Dictionary.Polygons.Services;
using Microsoft.AspNet.Identity;
using Domain.Dictionary.Drivers.Services;

namespace DAL.EF.Migrations
{
    public class Configuration : DbMigrationsConfiguration<ApplicationDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
            AutomaticMigrationDataLossAllowed = true;

        }

        protected override void Seed(ApplicationDbContext context)
        {
            base.Seed(context);

            var dataStore = new DataStore(context);

            CreateDirector(dataStore);

            CreateManager(dataStore);

            CreateDispatcher(dataStore);

            CreateTestData(dataStore);
        }

        private void CreateDirector(IDataStore dataStore)
        {
            if (dataStore.GetAll<UserRole>().Any(x => x.RoleId == Role.Director.ToString()))
            {
                return;
            }

            var userStore = new UserStore(dataStore);
            var userManager = new ApplicationUserManager(userStore);

            var directorUser = new ApplicationUser
            {
                Login = "director@tbo.com",
                UserName = "Директор",
                FullName = "Директор"
            };

            userManager.Create(directorUser, "pa$$w0rd");
            userManager.AddToRole(directorUser.Id, Role.Director.ToString());
        }

        private void CreateManager(IDataStore dataStore)
        {
            if (dataStore.GetAll<UserRole>().Any(x => x.RoleId == Role.Manager.ToString()) || !Settings.TestEnvironment)
            {
                return;
            }

            var userStore = new UserStore(dataStore);
            var userManager = new ApplicationUserManager(userStore);

            var managerUser = new ApplicationUser
            {
                Login = "manager@tbo.com",
                UserName = "Менеджер",
                FullName = "Менеджер"
            };

            userManager.Create(managerUser, "pa$$w0rd");
            userManager.AddToRole(managerUser.Id, Role.Manager.ToString());
        }

        private void CreateDispatcher(IDataStore dataStore)
        {
            if (dataStore.GetAll<UserRole>().Any(x => x.RoleId == Role.Dispatcher.ToString()) || !Settings.TestEnvironment)
            {
                return;
            }

            var userStore = new UserStore(dataStore);
            var userManager = new ApplicationUserManager(userStore);

            var managerUser = new ApplicationUser
            {
                Login = "dispatcher@tbo.com",
                UserName = "Диспетчер",
                FullName = "Диспетчер"
            };

            userManager.Create(managerUser, "pa$$w0rd");
            userManager.AddToRole(managerUser.Id, Role.Dispatcher.ToString());
        }

        private void CreateTestData(IDataStore dataStore)
        {
            ContainerTypeService.CreateTestData(dataStore);

            ContainerService.CreateTestData(dataStore);

            CarService.CreateTestData(dataStore);

            DriverService.CreateTestData(dataStore);

            PolygonService.CreateTestData(dataStore);

            CustomerService.CreateTestData(dataStore);
        }
    }
}
