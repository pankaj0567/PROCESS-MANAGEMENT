using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace PM.MVC.Utility
{
    public interface IHttpClientHelper<T> where T : class
    {
        Task<T> GetSingleItemRequest(string apiUrl, CancellationToken token = default(CancellationToken));
        Task<List<T>> GetMultipleItemsRequest(string apiUrl, CancellationToken token = default(CancellationToken));
        Task<T> PostRequest(string apiUrl, T postObject, CancellationToken token = default(CancellationToken));
        Task PutRequest(string apiUrl, T putObject, CancellationToken token = default(CancellationToken));
        Task DeleteRequest(string apiUrl, CancellationToken token = default(CancellationToken));
    }
}
