using ECommerce.Domain.Contracts;
using ECommerce.ServicesAbstarction;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace ECommerce.Services
{
    public class CacheServices : ICacheServices
    {
        private readonly ICacheRepository _casheRepository;
        public CacheServices(ICacheRepository casheRepository)
        {
            _casheRepository = casheRepository;
        }

        public async Task<string> GetAsync(string CasheKey)
        {
            return await _casheRepository.GetAsync(CasheKey);
        }

        public async Task SetAsync(string CasheKey, object CasheValue, TimeSpan TimeToLive)
        {
            var Value = JsonSerializer.Serialize(CasheValue);
            await _casheRepository.SetAsync(CasheKey, Value, TimeToLive);
        }
    }
}
