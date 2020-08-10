using Microsoft.Azure.Cosmos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InterviewProject
{
    public interface IDatabaseClient<T>
    {
        Task Reset();
        Task Init();
        Task DeleteItem(string partitionKey, string id);

        Task<ItemResponse<T>> UpsertItem(string partitionKey, T item);
        Task<ItemResponse<T>> GetItem(string partitionKey, string id);

        Task<List<T>> ExecuteQuery(string query);
    }
}
