using MongoDB.Driver;

namespace SchoolApi.Contracts;

public interface ICollectionServices
{
    Task CreateAsync<TEntity>(string name);
    Task DropAsync(string name);
    Task<List<string>> CollectionsNamesAsync();
    Task Rename<TEntity>(string oldName, string newName);
    IMongoCollection<TEntity> GetCollectionByName<TEntity>(string name);
    Task<long> DocumentsCount<TEntity>(string collectionName);
    Task<long> EstimateDocumentsCount<TEntity>(string collectionName);
}