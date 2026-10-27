using ECommerce.Domain.Contracts;
using ECommerce.Domain.Entities;
using ECommerce.Domain.Entities.OrderModules;
using ECommerce.Domain.Entities.ProductModule;
using ECommerce.Persistance.Data.DbContexts;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace ECommerce.Persistance.Data.DataSeed
{
    public class DataIntializer : IDataInitializer
    {
        private readonly StoreDbContext _dbContext;

        // CLR => inject obect from database context
        public DataIntializer(StoreDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task InitializeAsync()
        {
            var HasProducts = await _dbContext.Products.AnyAsync();
            var HasBrands = await _dbContext.ProductBrands.AnyAsync();
            var HasTypes = await _dbContext.ProductTypes.AnyAsync();
            var HasDelivery = await _dbContext.Set<DeliveryMethod>().AnyAsync();

            if (!HasBrands)
               await SeedDataFromJSONAsync<ProductBrand, int>("brands.json", _dbContext.ProductBrands);
            if (!HasTypes)
                await SeedDataFromJSONAsync<ProductType, int>("types.json", _dbContext.ProductTypes);
            _dbContext.SaveChanges();

            if (!HasProducts)
                await SeedDataFromJSONAsync<Product, int>("products.json", _dbContext.Products);

            if (!HasDelivery)
                await SeedDataFromJSONAsync<DeliveryMethod, int>("delivery.json", _dbContext.Set<DeliveryMethod>());
            _dbContext.SaveChanges();
        }

        public async Task SeedDataFromJSONAsync<T, TKey>(string fileName, DbSet<T> dbSet) where T : BaseEntity<TKey>
        {
            //D:\Route-ASP.Net\08 ASP.NET Core Web API\ECommerce - Project\ECommerce.Web.Solution\ECommerce.Persistance\Data\DataSeed\JSONFiles\
            var filePath = @"..\ECommerce.Persistance\Data\DataSeed\JSONFiles\" + fileName;

            if(!File.Exists(filePath))
                throw new FileNotFoundException($"JSON file not found : {fileName}");

            ///Small json file
            ///var jsonData = File.ReadAllText(filePath);

            try
            {
                using var DataStream = File.OpenRead(filePath);

                var Data = JsonSerializer.Deserialize<List<T>>(DataStream);

                if (Data is not null)
                {
                   await dbSet.AddRangeAsync(Data);
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed to seed Data : {ex}");
            }
        }
    }
}
