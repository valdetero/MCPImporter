using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading.Tasks;
using MCPImporter.Data;
using MCPImporter.Common;
using System.Linq;

namespace MCPImporter.Business.Application
{
    public abstract class ApplicationBCBase<E> : IEntityBC<E>
    {
        internal IMCPImporterUnitOfWork uow;

        internal abstract IRepository<E> repository
        {
            get;
        }

        protected ApplicationBCBase(IMCPImporterUnitOfWork uow)
        {
            this.uow = uow;
        }

        protected ApplicationBCBase()
            : this(new MCPImporterUnitOfWork())
        {

        }

        public virtual void Add(E entity)
        {
            repository.Add(entity);
        }

        public virtual void Add(IEnumerable<E> entities)
        {
            //await Task.Run(() => Parallel.ForEach(entities, Add));
            //await Task.Run(() =>
            //                   {
                                   //await entities.ParallelForEachAsync(Add, null);
            //                   }
            //    )
            //;

            foreach (var entity in entities)
            {
                this.Add(entity);
            }

        }

        public virtual void Delete(E entity)
        {
            repository.Delete(entity);
        }

        public virtual void Update(E entity)
        {
            repository.Update(entity);
        }

        public virtual Task Save()
        {
            return repository.Save();
        }

        public virtual Task<E> GetByID(object id)
        {
            return repository.GetByID(id);
        }

        public virtual Task<IList<E>> GetAll()
        {
            return repository.All().ToListAsync();
        }
    }
}
