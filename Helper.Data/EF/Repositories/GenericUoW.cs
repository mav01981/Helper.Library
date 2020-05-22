namespace Helper.Data.EF
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Helper.Data.EF.Repositories;
    using Microsoft.EntityFrameworkCore;

    public class GenericUoW : IUnitOfWork
    {
        private readonly DbContext entities = null;

        private readonly Dictionary<Type, object> repositories = new Dictionary<Type, object>();

        public GenericUoW(DbContext entities)
        {
            this.entities = entities;
        }

        public IRepository<T> Repository<T>()
            where T : class
        {
            if (this.repositories.Keys.Contains(typeof(T)) == true)
            {
                return this.repositories[typeof(T)] as IRepository<T>;
            }

            IRepository<T> repository = new GenericRepository<T>(this.entities);
            this.repositories.Add(typeof(T), repository);
            return repository;
        }

        public void SaveChanges()
        {
            this.entities.SaveChanges();
        }
    }
}
