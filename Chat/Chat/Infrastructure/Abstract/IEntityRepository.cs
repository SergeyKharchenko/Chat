using System.Linq;

namespace Chat.Infrastructure.Abstract
{
    public interface IEntityRepository<TEntity> where TEntity : class
    {
        IQueryable<TEntity> Entities { get; }

        TEntity GetById(int id);
        void Create(TEntity entity);
        void Update(TEntity entity);
        void Delete(TEntity entity);
        void Save(); 
    }
}