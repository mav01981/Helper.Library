namespace Helper.Data.EF
{
    using Helper.Data.EF.Repositories;

    public interface IUnitOfWork
    {
        IRepository<T> Repository<T>() where T : class;

        void SaveChanges();
    }
}
