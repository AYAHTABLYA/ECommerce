using ECommerce.Domain.Contracts;
using ECommerce.Persistance.Data.DbContexts;
using ECommerce.Persistance.IdentityData.DbContexts;
using Microsoft.EntityFrameworkCore;

namespace ECommerce.Web.Extentions
{
    public static class WebAppRegesteration
    {
        public static async Task<WebApplication> MigrateDbAsync(this WebApplication app)
        {
           await using var scope = app.Services.CreateAsyncScope();
            var dbContextService = scope.ServiceProvider.GetRequiredService<StoreDbContext>();

            var pendingMigrations = await dbContextService.Database.GetPendingMigrationsAsync();
            if (pendingMigrations.Any())
               await dbContextService.Database.MigrateAsync();

            return app;
        }
        public static async Task<WebApplication> MigrateIdentityDbAsync(this WebApplication app)
        {
           await using var scope = app.Services.CreateAsyncScope();
            var dbContextService = scope.ServiceProvider.GetRequiredService<StoreIdentityDbContext>();

            var pendingMigrations = await dbContextService.Database.GetPendingMigrationsAsync();
            if (pendingMigrations.Any())
               await dbContextService.Database.MigrateAsync();

            return app;
        }
        
        public static async Task<WebApplication> SeedDataAsync(this WebApplication app)
        {
           await using var scope = app.Services.CreateAsyncScope();
            var dataInitializerService = scope.ServiceProvider.GetRequiredKeyedService<IDataInitializer>("Default");
           await dataInitializerService.InitializeAsync();

            return app;
        }

        public static async Task<WebApplication> SeedIdentityDataAsync(this WebApplication app)
        {
           await using var scope = app.Services.CreateAsyncScope();
            var dataInitializerService = scope.ServiceProvider.GetRequiredKeyedService<IDataInitializer>("Identity");
           await dataInitializerService.InitializeAsync();

            return app;
        }
    }
}
