using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRMService.Core.Repositories.Base
{
    public interface IRepository<TModel> where TModel : class
    {
        IQueryable<TModel> GetDbQueryable();     

        void AddDb(TModel entity);
        IEnumerable<TModel> GetDbAll();

        void Add(TModel entity);
        void Remove(TModel entity);

        Task<bool> SaveChangesAsync();
    }
}
