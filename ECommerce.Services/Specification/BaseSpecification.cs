using ECommerce.Domain.Contracts;
using ECommerce.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Services.Specification
{
    public class BaseSpecification<TEntity, TKey> : ISpecification<TEntity, TKey> where TEntity : BaseEntity<TKey>
    {
        #region Includes
        public ICollection<Expression<Func<TEntity, object>>> IncludeExpression { get; } = [];
        protected void AddInclude(Expression<Func<TEntity, object>> includeExpression)
        {
            IncludeExpression.Add(includeExpression);

        }
        #endregion

        #region Where
        public Expression<Func<TEntity, bool>> Ctiteria { get; }

        public BaseSpecification(Expression<Func<TEntity, bool>> CtiteriaExp)
        {
            Ctiteria = CtiteriaExp;
        }

        #endregion

        #region Sorting [OrderBy]

        public Expression<Func<TEntity, object>> OrderBy { get; private set; }

        public Expression<Func<TEntity, object>> OrderByDesc { get; private set; }

        protected void AddOrderBy(Expression<Func<TEntity, object>> orderByExpression)
        {
            OrderBy = orderByExpression;
        }
        protected void AddOrderByDesc(Expression<Func<TEntity, object>> orderByDescExpression)
        {
            OrderByDesc = orderByDescExpression;
        }
        #endregion

        #region Pagination
        public int Take { get; private set; }
        public int Skip { get; private set; }
        public bool IsPaginated { get; set; }

        protected void ApplyyPagination(int pageSize, int pageIndex)
        {
            IsPaginated = true;
            Take = pageSize;
            Skip = pageSize * (pageIndex - 1);
        }
        #endregion

    }
}
