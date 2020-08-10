using Microsoft.Azure.Cosmos;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InterviewProject
{
    public class DatabaseClient<T> : IDatabaseClient<T>
    {
        private ILogger logger;
        private DatabaseClientConfiguration config;
        private string containerName;
        private string partition;
        private CosmosClient client;
        private Database database = null;
        private Container container = null;

        public DatabaseClient(ILogger<DatabaseClient<T>> logger, DatabaseClientConfiguration config, string containerName, string partition)
        {
            this.config = config;
            this.containerName = containerName;
            this.partition = partition;
            this.logger = logger;
            this.client = new CosmosClient(config.EndpointUri, config.PrimaryKey);
        }

        public async Task Init()
        {
            if (this.database == null)
                this.database = await this.client.CreateDatabaseIfNotExistsAsync(this.config.Database);
            if (this.container == null)
                this.container = await this.database.CreateContainerIfNotExistsAsync(this.containerName, this.partition);
        }

        public async Task DeleteItem(string partitionKey, string id)
        {
            await this.container.DeleteItemAsync<T>(id, new PartitionKey(partitionKey));
            this.logger.LogInformation($"deleted {database}.{container} partition {partitionKey} id: {id}");
        }
        public async Task<ItemResponse<T>> UpsertItem(string partitionKey, T item)
        {
            var response = await this.container.UpsertItemAsync<T>(item, new PartitionKey(partitionKey));
            this.logger.LogInformation($"upsert {database}.{container} partition {partitionKey} item: {JsonConvert.SerializeObject(item)}");
            return response;
        }

        public async Task<ItemResponse<T>> GetItem(string partitionKey, string id)
        {
            var response = await this.container.ReadItemAsync<T>(id, new PartitionKey(partitionKey));
            this.logger.LogInformation($"read item {database}.{container} partition {partitionKey} item: {JsonConvert.SerializeObject(response.Resource)}");
            return response;
        }

        public async Task<List<T>> ExecuteQuery(string query)
        {
            var queryDefinition = new QueryDefinition(query);
            FeedIterator<T> queryResultSetIterator = this.container.GetItemQueryIterator<T>(queryDefinition);
            List<T> response = new List<T>();

            while (queryResultSetIterator.HasMoreResults)
            {
                FeedResponse<T> currentResultSet = await queryResultSetIterator.ReadNextAsync();
                foreach (T item in currentResultSet)
                {
                    response.Add(item);
                }
            }

            this.logger.LogInformation($"executed query {query} and retrieved {response.Count} items");

            return response;
        }

        public async Task Reset()
        {
            await this.container.DeleteContainerAsync();
            this.container = null;
            await this.Init();
        }

    }
    public class DatabaseClientConfiguration
    {
        public string EndpointUri { get; set; }
        public string PrimaryKey { get; set; }
        public string Database { get; set; }
    }
}
