using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using Chat.Infrastructure.Abstract;

namespace Chat.Infrastructure.Concrete
{
    public class Repository<TEntity> : IRepository<TEntity> where TEntity : class
    {
        private readonly DbContext context;
        private readonly IDbSet<TEntity> dbSet;

        public Repository(DbContext context, IDbSet<TEntity> dbSet)
        {
            this.context = context;
            this.dbSet = dbSet;
        }

        public IEnumerable<TEntity> Entities { get { return dbSet.AsEnumerable(); } }

        public IEnumerable<TEntity> FindBy(Expression<Func<TEntity, bool>> filterCriterion,
                                           params Expression<Func<TEntity, object>>[] includeCriterion)
        {
            var entities = dbSet.Where(filterCriterion);
            if (includeCriterion != null)
                entities = includeCriterion.Aggregate(entities, (current, expression) => current.Include(expression));
            return entities;
        }

        public TEntity FindById(int id)
        {
            return dbSet.Find(id);
        }

        public void Add(TEntity entity)
        {
            dbSet.Add(entity);
        }

        public void Update(TEntity entity)
        {
            dbSet.Attach(entity);
            context.Entry(entity).State = EntityState.Modified;
        }

        public void Remove(TEntity entity)
        {
            if (entity == null)
                return;
            dbSet.Remove(entity);
        }
    }
}