using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using Entities.Models;

namespace Chat.Tests.Dummy
{
    public class DummyDbSet<TEntity> : IDbSet<TEntity> where TEntity : Entity
    {
        private readonly List<TEntity> entities;
        private readonly IQueryable<TEntity> queryable;

        public DummyDbSet()
            : this(Enumerable.Empty<TEntity>())
        {
        }

        public DummyDbSet(IEnumerable<TEntity> entities)
        {
            this.entities = new List<TEntity>(entities);
            queryable = this.entities.AsQueryable();
        }

        public IEnumerator<TEntity> GetEnumerator()
        {
            return entities.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public Expression Expression
        {
            get { return queryable.Expression; }
        }

        public Type ElementType
        {
            get { return queryable.ElementType; }
        }

        public IQueryProvider Provider
        {
            get { return queryable.Provider; }
        }

        public TEntity Find(params object[] keyValues)
        {
            var id = (int) keyValues.First();
            return entities.First(entity => entity.Id == id);
        }

        public TEntity Add(TEntity entity)
        {
            entities.Add(entity);
            return entity;
        }

        public TEntity Remove(TEntity entity)
        {
            entities.Remove(entity);
            return entity;
        }

        public TEntity Attach(TEntity entity)
        {
            entities.Add(entity);
            return entity;
        }

        public TEntity Create()
        {
            throw new NotImplementedException();
        }

        public TDerivedEntity Create<TDerivedEntity>() where TDerivedEntity : class, TEntity
        {
            throw new NotImplementedException();
        }

        public ObservableCollection<TEntity> Local { get; private set; }
    }
}