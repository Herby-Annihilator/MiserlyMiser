using MiserlyMiser.Models.Entities.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MiserlyMiser.Models.Repositories.Interfaces
{
    public interface ICrudRepository<T> where T : Entity
    {
        T GetById(int id);
        Task<T> GetByIdAsync(int id, CancellationToken cancellationToken = default);
        ICollection<T> GetAll();
        Task<ICollection<T>> GetAllAsync(CancellationToken cancellationToken = default);
        void Delete(T entity);
        Task DeleteAsync(T entity, CancellationToken cancellationToken = default);
        void Update(T entity);
        Task UpdateAsync(T entity, CancellationToken cancellationToken = default);
        void Create(T entity);
        Task CreateAsync(T entity, CancellationToken cancellationToken = default);
    }
}
