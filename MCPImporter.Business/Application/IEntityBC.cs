using System.Collections.Generic;
using System.Threading.Tasks;

namespace MCPImporter.Business.Application
{
    public interface IEntityBC<E>
    {
        void Add(E entity);
        void Add(IEnumerable<E> entities);
        void Delete(E entity);
        void Update(E entity);
        Task Save();	
        Task<E> GetByID(object id);
        //Task<IEnumerable<E>> GetAll();
        Task<IList<E>> GetAll();
    }
}
