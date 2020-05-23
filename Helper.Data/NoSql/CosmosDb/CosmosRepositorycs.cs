namespace Helper.Data.Cosmos
{
    using System;
    using System.Threading.Tasks;
    using Microsoft.Azure.Documents;
    using Microsoft.Azure.Documents.Client;
    using Newtonsoft.Json;

    public abstract class CosmosDbRepository<T> where T : Entity
    {
        private readonly ICosmosDbClientFactory _cosmosDbClientFactory;

        protected CosmosDbRepository(ICosmosDbClientFactory cosmosDbClientFactory)
        {
            _cosmosDbClientFactory = cosmosDbClientFactory;
        }

        public async Task<T> GetByIdAsync(string id)
        {
            try
            {
                var cosmosDbClient = _cosmosDbClientFactory.GetClient(CollectionName);
                var document = await cosmosDbClient.ReadDocumentAsync(id,
                    new RequestOptions
                    {
                        PartitionKey = this.ResolvePartitionKey(id),
                    });

                return JsonConvert.DeserializeObject<T>(document.ToString());
            }
            catch (DocumentClientException)
            {
                throw;
            }
        }

        public async Task<T> AddAsync(T entity)
        {
            try
            {
                entity.Id = GenerateId(entity);
                var cosmosDbClient = _cosmosDbClientFactory.GetClient(CollectionName);
                var document = await cosmosDbClient.CreateDocumentAsync(entity);
                return JsonConvert.DeserializeObject<T>(document.ToString());
            }
            catch (DocumentClientException)
            {
                throw;
            }
        }

        public async Task UpdateAsync(T entity)
        {
            try
            {
                var cosmosDbClient = _cosmosDbClientFactory.GetClient(CollectionName);
                await cosmosDbClient.ReplaceDocumentAsync(entity.Id, entity);
            }
            catch (DocumentClientException)
            {
                throw;
            }
        }

        public async Task DeleteAsync(T entity)
        {
            try
            {
                var cosmosDbClient = _cosmosDbClientFactory.GetClient(CollectionName);
                await cosmosDbClient.DeleteDocumentAsync(entity.Id, new RequestOptions
                {
                    PartitionKey = ResolvePartitionKey(entity.Id),
                });
            }
            catch (DocumentClientException)
            {
                throw;
            }
        }

        public abstract string CollectionName { get; }

        public virtual string GenerateId(T entity) => Guid.NewGuid().ToString();

        public virtual PartitionKey ResolvePartitionKey(string entityId) => null;
    }
}
