using ECommerce.Domain.Contracts;
using ECommerce.Domain.Entities.BasketModule;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace ECommerce.Persistance.Repositories
{
    public class BasketRepository : IBasketRepository
    {
        private readonly IDatabase _database;

        public BasketRepository(IConnectionMultiplexer connection)
        {
            _database = connection.GetDatabase();
        }

        public async Task<CustomerBasket> CreateorUpdateBasketAsync(CustomerBasket basket, TimeSpan timeToLive = default)
        {
            var JsonBasket = JsonSerializer.Serialize(basket);
            var IsCreatedorupdated = await _database.StringSetAsync(basket.Id, JsonBasket, (timeToLive == default) ? TimeSpan.FromDays(7) : timeToLive);

            if (IsCreatedorupdated)
            {
                var Basket = await _database.StringGetAsync(basket.Id);
                return JsonSerializer.Deserialize<CustomerBasket>(Basket!);
            }
            else
            {
                return null;
            }
        }

        public async Task<bool> DeleteBasketAsync(string id)
        {
            return await _database.KeyDeleteAsync(id);
        }

        public async Task<CustomerBasket?> GetBasketAsync(string id)
        {
            var Basket = await _database.StringGetAsync(id);
            if (Basket.IsNullOrEmpty)
                return null;
            else
                return JsonSerializer.Deserialize<CustomerBasket>(Basket!);
        }
    }
}
