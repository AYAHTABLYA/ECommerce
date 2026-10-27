using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.ServicesAbstarction
{
    public interface ICacheServices
    {
        Task<string> GetAsync(string CasheKey);
        Task SetAsync(string CasheKey, object CasheValue, TimeSpan TimeToLive);
    }
}
