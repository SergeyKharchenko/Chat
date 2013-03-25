using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Chat.Models;

namespace Chat.Infrastructure.Abstract
{
    public interface IRepository<TEntity> where TEntity : Entity
    {
        IEnumerable<TEntity> Entities { get; }

        IEnumerable<TEntity> FindBy(Expression<Func<TEntity, bool>> filterCriterion,
                                    params Expression<Func<TEntity, object>>[] includeCriterion);
        TEntity FindById(int id);

        void Add(TEntity entity);
        void Remove(TEntity entity);
    }
}