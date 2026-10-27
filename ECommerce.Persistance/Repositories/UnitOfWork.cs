using ECommerce.Domain.Contracts;
using ECommerce.Domain.Entities;
using ECommerce.Persistance.Data.DbContexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Persistance.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly StoreDbContext _dbContext;
        private readonly Dictionary<Type, object> _repositories = [];

        public UnitOfWork(StoreDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public IGenericRepository<TEntity, TKey> GetRepository<TEntity, TKey>() where TEntity : BaseEntity<TKey>
        {
            var EntityType = typeof(TEntity);
            if (_repositories.TryGetValue(EntityType, out var repo))
            {
                return (IGenericRepository<TEntity, TKey>) repo;
            }

            var newRepo = new GenericRepository<TEntity, TKey>(_dbContext);
            _repositories[EntityType] = newRepo;

            return newRepo;
        }

        public async Task<int> SaveChangesAsync()
            => await _dbContext.SaveChangesAsync();
    }
}
