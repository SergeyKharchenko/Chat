using System.Data;
using System.Data.Entity;
using System.Linq;
using Chat.Infrastructure.Abstract;
using Entities.Core.Concrete;

namespace Chat.Infrastructure.Concrete
{
    public class EntityRepository<TEntity> : IEntityRepository<TEntity> where TEntity : class
    {
        private readonly ChatContext context;
        private readonly DbSet<TEntity> dbSet;

        public EntityRepository(ChatContext context)
        {
            this.context = context;
            dbSet = context.Set<TEntity>();
        }

        public IQueryable<TEntity> Entities { get { return dbSet; } }

        public TEntity GetById(int id)
        {
            return dbSet.Find(id);
        }

        public void Create(TEntity entity)
        {
            dbSet.Add(entity);
            Save();
        }

        public void Update(TEntity entity)
        {
            dbSet.Attach(entity);
            context.Entry(entity).State = EntityState.Modified;
            Save();
        }

        public void Delete(TEntity entity)
        {
            if (entity == null)
                return;
            if (context.Entry(entity).State == EntityState.Detached)
                dbSet.Attach(entity);
            dbSet.Remove(entity);
            Save();
        }

        public void Save()
        {
            context.SaveChanges();
        }
    }
}