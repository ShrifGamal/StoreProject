using Store.Core.Entites;
using Store.Core.Specifications;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Core.RepositoriesContract
{
    public interface IGenericRepository<TEntity , TKey> where TEntity : BaseEntity<TKey>
    {
        Task<IEnumerable<TEntity>> GetAllAsync();

        Task<TEntity> GetByIdAsync(TKey id);
        Task<IEnumerable<TEntity>> GetAllWithSpecAsync(ISpecifications<TEntity ,TKey> spec);

        Task<int> GetCountAsync(ISpecifications<TEntity, TKey> spec);
        Task<TEntity> GetByIdWithSpecAsync(ISpecifications<TEntity, TKey> spec);

        Task AddAsync(TEntity entity);

        void Updata(TEntity entity);
        
        void Delete(TEntity entity);


    }
}
