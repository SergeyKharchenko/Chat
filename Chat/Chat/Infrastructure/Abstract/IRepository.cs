using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Chat.Infrastructure.Abstract
{
    public interface IRepository<TEntity> where TEntity : class
    {
        IEnumerable<TEntity> Entities { get; }

        IEnumerable<TEntity> FindBy(Expression<Func<TEntity, bool>> filterCriterion,
                                    params Expression<Func<TEntity, object>>[] includeCriterion);
        TEntity FindById(int id);

        void Add(TEntity entity);
        void Update(TEntity entity);
        void Remove(TEntity entity);
    }
}