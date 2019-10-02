using System;
using System.Threading.Tasks;

namespace Helper.Cache
{
    public interface ICache<T>
    {
        Task<T> GetOrCreate(object key, Func<Task<T>> createItem);
    }
}