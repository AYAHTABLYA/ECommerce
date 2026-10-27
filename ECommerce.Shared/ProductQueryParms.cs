using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Shared
{
    public class ProductQueryParms
    {
        public int? BrandId { get; set; }
        public int? TypeId { get; set; }
        public string? Search { get; set; }

        public ProductSortingOptions? Sorting { get; set; }

        private int _pageIndex = 1;

        public int _PageIndex
        {
            get { return _pageIndex = 1; }
            set { _pageIndex = (value <= 0) ? 1 : value; }
        }


        private const int defaultPageSize = 5;
        private const int maxPageSize = 10;
        private int _pageSize = 10;
        
        public int _PageSize
        {
            get { return _pageSize; }
            set 
            {
                if (value <= 0)
                    _pageSize = defaultPageSize;
                else if (value > maxPageSize)
                    _pageSize = maxPageSize;
                else
                    _pageSize = value;
            }
        }
    }
}
