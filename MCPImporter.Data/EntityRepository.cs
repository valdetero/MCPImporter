using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace MCPImporter.Data
{
    [ExcludeFromCodeCoverage]
    public class EntityRepository<E> : IRepository<E> where E : class
    {
        protected readonly DbContext context;
        protected readonly DbSet<E> dbSet;

        public EntityRepository(DbContext context)
        {
            #region Argument Validation

            if (context == null)
            {
                throw new ArgumentNullException("context");
            }

            #endregion

            this.context = context;
            this.dbSet = context.Set<E>();
        }

        public virtual IQueryable<E> All()
        {
            return dbSet;
        }

        public virtual IQueryable<E> All(string includes)
        {
            IQueryable<E> value = dbSet;
            if (!String.IsNullOrEmpty(includes))
            {
                value = includes
                    .Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries)
                    .Aggregate(value, (current, includeProperty) => current.Include(includeProperty.Trim()));
            }
            return value;
        }

        public async virtual Task<IEnumerable<E>> Get(
            Expression<Func<E, bool>> filter = null,
            Func<IQueryable<E>, IOrderedQueryable<E>> orderBy = null,
            string includeProperties = "")
        {
            IQueryable<E> query = dbSet;

            if (filter != null)
            {
                query = query.Where(filter);
            }

            query = includeProperties
                .Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries)
                .Aggregate(query, (current, includeProperty) => current.Include(includeProperty));

            if (orderBy != null)
            {
                return await orderBy(query).ToListAsync();
            }
            else
            {
                return await query.ToListAsync();
            }
        }

        public virtual Task<E> GetByID(object id)
        {
            return dbSet.FindAsync(id);
        }

        public virtual void Add(E entity)
        {
            #region Argument Validation

            if (entity == null)
            {
                throw new ArgumentNullException("entity");
            }

            #endregion

            dbSet.Add(entity);
        }

        public virtual void Update(E entity)
        {
            dbSet.Attach(entity);
            context.Entry(entity).State = EntityState.Modified;
        }

        public virtual void Delete(E entity)
        {
            #region Argument Validation

            if (entity == null)
            {
                throw new ArgumentNullException("entity");
            }

            #endregion

            if (context.Entry(entity).State == EntityState.Detached)
            {
                dbSet.Attach(entity);
            }

            dbSet.Remove(entity);
        }

        public virtual Task Save()
        {
            return context.SaveChangesAsync();
        }

        public void SetLazyLoadingOption(bool isEnabled)
        {
            context.Configuration.LazyLoadingEnabled = false;
        }
    }
}
