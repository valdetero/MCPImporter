using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace MCPImporter.Data
{
    public interface IRepository<E>
    {
        IQueryable<E> All();
        IQueryable<E> All(string includes);
        void Add(E entity);
        void Delete(E entity);
        void Update(E entity);
        Task Save();

        Task<IEnumerable<E>> Get(Expression<Func<E, bool>> filter = null, Func<IQueryable<E>, IOrderedQueryable<E>> orderBy = null, string includeProperties = "");
        Task<E> GetByID(object id);
    }
}
