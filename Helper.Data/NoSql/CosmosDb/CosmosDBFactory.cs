﻿namespace Helper.Data.Cosmos
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Microsoft.Azure.Documents;
    using Microsoft.Azure.Documents.Client;

    public class CosmosDbClientFactory : ICosmosDbClientFactory
    {
        private readonly string _databaseName;
        private readonly List<string> _collectionNames;
        private readonly IDocumentClient _documentClient;

        /// <summary>
        /// Initializes a new instance of the <see cref="CosmosDbClientFactory"/> class.
        /// </summary>
        /// <param name="databaseName"></param>
        /// <param name="collectionNames"></param>
        /// <param name="documentClient"></param>
        public CosmosDbClientFactory(
            string databaseName, List<string> collectionNames, IDocumentClient documentClient)
        {
            _databaseName = databaseName ?? throw new ArgumentNullException(nameof(databaseName));
            _collectionNames = collectionNames ?? throw new ArgumentNullException(nameof(collectionNames));
            _documentClient = documentClient ?? throw new ArgumentNullException(nameof(documentClient));
        }

        public ICosmosDbClient GetClient(string collectionName)
        {
            if (!_collectionNames.Contains(collectionName))
            {
                throw new ArgumentException($"Unable to find collection: {collectionName}");
            }

            return new CosmosDbClient(_databaseName, collectionName, _documentClient);
        }

        public async Task EnsureDbSetupAsync()
        {
            await _documentClient.ReadDatabaseAsync(UriFactory.CreateDatabaseUri(_databaseName));

            foreach (var collectionName in _collectionNames)
            {
                await _documentClient.ReadDocumentCollectionAsync(
                    UriFactory.CreateDocumentCollectionUri(_databaseName, collectionName));
            }
        }
    }
}
