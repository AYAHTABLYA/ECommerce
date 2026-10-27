using ECommerce.Domain.Contracts;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Persistance.Repositories
{
    public class CacheRepository : ICacheRepository
    {
        private readonly IDatabase _database;

        public CacheRepository(IConnectionMultiplexer connection)
        {
            _database = connection.GetDatabase();
        }

        public async Task<string?> GetAsync(string CasheKey)
        {
            var CasheValue = await _database.StringGetAsync(CasheKey);
            if (CasheValue.IsNullOrEmpty) return null;

            return CasheValue.ToString();
        }

        public async Task SetAsync(string CasheKey, string CasheValue, TimeSpan TimeToLive)
        {
            await _database.StringSetAsync(CasheKey, CasheValue, TimeToLive);
        }
    }
}
