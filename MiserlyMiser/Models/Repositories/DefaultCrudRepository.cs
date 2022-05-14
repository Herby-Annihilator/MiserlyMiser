using Microsoft.EntityFrameworkCore;
using MiserlyMiser.Models.DataContexts;
using MiserlyMiser.Models.Entities.Base;
using MiserlyMiser.Models.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MiserlyMiser.Models.Repositories
{
    public class DefaultCrudRepository<T> : ICrudRepository<T> where T : Entity
    {
        protected MiserlyMiserDataContext _context;
        protected DbSet<T> EntitySet => _context.Set<T>();
        public DefaultCrudRepository(MiserlyMiserDataContext context)
        {
            _context = context;
        }
        public void Create(T entity)
        {
            EntitySet.Add(entity);
            _context.SaveChanges();
        }

        public void Delete(T entity)
        {
            EntitySet.Remove(entity);
            _context.SaveChanges();
        }

        public ICollection<T> GetAll()
        {
            return EntitySet.ToArray();
        }

        public T GetById(int id) => EntitySet.FirstOrDefault(entity => entity.Id == id);

        public void Update(T entity)
        {
            EntitySet.Update(entity);
            _context.SaveChanges();
        }

        public async Task<T> GetByIdAsync(int id, CancellationToken cancellationToken = default) 
            => await EntitySet.FirstOrDefaultAsync(entity => entity.Id == id, cancellationToken)
            .ConfigureAwait(false);

        public async Task<ICollection<T>> GetAllAsync(CancellationToken cancellationToken = default)
            => await EntitySet.ToArrayAsync(cancellationToken).ConfigureAwait(false);

        public async Task DeleteAsync(T entity, CancellationToken cancellationToken = default)
        {
            EntitySet.Remove(entity);
            await _context.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
            return;
        }

        public async Task UpdateAsync(T entity, CancellationToken cancellationToken = default)
        {
            EntitySet.Update(entity);
            await _context.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
        }

        public async Task CreateAsync(T entity, CancellationToken cancellationToken = default)
        {
            await EntitySet.AddAsync(entity, cancellationToken).ConfigureAwait(false);
            await _context.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
        }
    }
}
