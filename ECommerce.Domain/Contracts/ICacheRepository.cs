using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Domain.Contracts
{
    public interface ICacheRepository
    {
        Task<string?> GetAsync(string CasheKey);
        Task SetAsync(string CasheKey, string CasheValue, TimeSpan TimeToLive);
    }
}
