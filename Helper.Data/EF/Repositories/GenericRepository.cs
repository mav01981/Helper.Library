namespace Helper.Data.EF.Repositories
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Microsoft.EntityFrameworkCore;

    /// <summary>
    /// Generic Repository.
    /// </summary>
    /// <typeparam name="T">Type Of T.</typeparam>
    public class GenericRepository<T> : IRepository<T>
        where T : class
    {
        private readonly DbContext _entities = null;
        private DbSet<T> _objectSet;

        public GenericRepository(DbContext entities)
        {
            _entities = entities;
            _objectSet = entities.Set<T>();
        }

        /// <summary>
        /// Get all the list.
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public IEnumerable<T> GetAll(Func<T, bool> predicate = null)
        {
            if (predicate != null)
            {
                return this._objectSet.Where(predicate);
            }

            return this._objectSet.AsEnumerable();
        }

        /// <summary>
        /// Get a certain element of the list.
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns>Object.</returns>
        public T Get(Func<T, bool> predicate)
        {
            return this._objectSet.First(predicate);
        }

        /// <summary>
        /// Add a new element to the list.
        /// </summary>
        /// <param name="entity"></param>
        public void Add(T entity)
        {
            this._objectSet.Add(entity);
        }

        /// <summary>
        /// Update a certain element.
        /// </summary>
        /// <param name="entity"></param>
        public void Update(T entity)
        {
            this._objectSet.Update(entity);
        }

        /// <summary>
        /// Remove an element from the list.
        /// </summary>
        /// <param name="entity"></param>
        public void Remove(T entity)
        {
            this._objectSet.Remove(entity);
        }
    }
}
