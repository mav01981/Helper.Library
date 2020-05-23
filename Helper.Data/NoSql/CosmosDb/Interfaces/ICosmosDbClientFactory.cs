namespace Helper.Data.Cosmos
{
    using System.Threading.Tasks;

    public interface ICosmosDbClientFactory
    {
        ICosmosDbClient GetClient(string collectionName);

        Task EnsureDbSetupAsync();
    }
}