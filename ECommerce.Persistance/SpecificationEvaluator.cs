using ECommerce.Domain.Contracts;
using ECommerce.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace ECommerce.Persistance
{
    public class SpecificationEvaluator
    {
        public static IQueryable<TEntity> CreateQuery<TEntity, TKey>(IQueryable<TEntity> entrypoint,
                                       ISpecification<TEntity, TKey> specification) where TEntity : BaseEntity<TKey>
        {
            var Query = entrypoint;

            if (specification is not null)
            {
                // Where
                if (specification.Ctiteria is not null)
                {
                    Query = Query.Where(specification.Ctiteria);
                }

                // Include expressions
                if (specification.IncludeExpression is not null && specification.IncludeExpression.Any())
                {
                    ///foreach (var includeExpression in specification.IncludeExpression)
                    ///{
                    ///    Query = Query.Include(includeExpression);
                    ///}
                
                    Query = specification.IncludeExpression
                               .Aggregate(Query, (current, includeExpression) => current.Include(includeExpression));
                }

                // OrderBy
                if (specification.OrderBy is not null)
                {
                    Query = Query.OrderBy(specification.OrderBy);
                }

                // OrderByDesc
                if (specification.OrderByDesc is not null)
                {
                    Query = Query.OrderByDescending(specification.OrderByDesc);
                }

                // Pagination
                if (specification.IsPaginated)
                {
                    Query = Query.Skip(specification.Skip).Take(specification.Take);
                }
            }
            return Query;
        }
    }
}
