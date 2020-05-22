namespace Helper.Data.EF.Repositories
{
    using System;
    using System.Collections.Generic;

    public interface IRepository<T>
        where T : class
    {
        IEnumerable<T> GetAll(Func<T, bool> predicate = null);

        T Get(Func<T, bool> predicate);

        void Add(T entity);

        void Update(T entity);

        void Remove(T entity);
    }
}