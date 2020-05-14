using CRMService.Core.Repositories.Base;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRMService.Infrastructure.Data.EntityFramework.Repositories.Abstract
{
    public abstract class Repository<TModel> : IRepository<TModel> where TModel : class
    {
        protected readonly DbContext DatabaseContext;
        protected readonly DbSet<TModel> _dbSet;      

        public Repository(DbSet<TModel> dbSet)
        {
            _dbSet = dbSet;
        }

        public Repository(DbContext context)
        {
            this.DatabaseContext = context;
        }

        public IQueryable<TModel> GetDbQueryable()
        {
            return _dbSet;
        }

        public void AddDb(TModel entity)
        {
            _dbSet.Add(entity);
        }

        public IEnumerable<TModel> GetDbAll()
        {
            return _dbSet.AsQueryable().ToList();
        }

        public async Task<bool> SaveChangesAsync()
        {
            // Return success if at least one row was changed
            return (await DatabaseContext.SaveChangesAsync()) > 0;
        }

        public void Add(TModel entity)
        {
            DatabaseContext.Set<TModel>().Add(entity);
        }

        public IEnumerable<TModel> GetAll()
        {
            return DatabaseContext.Set<TModel>().ToList();
        }

        public void Remove(TModel entity)
        {
            DatabaseContext.Set<TModel>().Remove(entity);
        }
    }
}
