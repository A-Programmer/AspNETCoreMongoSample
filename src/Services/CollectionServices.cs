using Microsoft.Extensions.Options;
using MongoDB.Driver;
using SchoolApi.Models;

namespace SchoolApi.Services;

public class CollectionServices
{
    private readonly IMongoDatabase _database;

    public CollectionServices(IMongoClient client,
        IOptions<SchoolDatabaseSettings> schoolSettings)
    {
        _database = client.GetDatabase(schoolSettings.Value.DatabaseName);
    }

    public async Task CreateAsync<TEntity>(string name)
    {
        try
        {
            if (CheckCollectionExistence<TEntity>(name))
            {
                throw new Exception($"The collection with the name {name} exist.");
            }
            await _database.CreateCollectionAsync(name);
        }
        catch (Exception e)
        {
            throw new Exception($"Failed to create collection {name}. Error: {e.Message}");
        }
    }

    public async Task DropAsync(string name)
    {
        try
        {
            await _database.DropCollectionAsync(name);
        }
        catch (Exception e)
        {
            throw new Exception($"Failed to drop collection {{name}}. Error: {e.Message}");
        }
    }

    public async Task<List<string>> CollectionsNamesAsync()
    {
        try
        {
            var cursor = await _database.ListCollectionNamesAsync();
            var collectionsNames = await cursor.ToListAsync();
            return collectionsNames;
        }
        catch (Exception e)
        {
            throw new Exception($"Failed to retrieve collection names. Error {e.Message}");
        }
    }

    public async Task Rename<TEntity>(string oldName, string newName)
    {
        try
        {
            if (CheckCollectionExistence<TEntity>(newName))
            {
                throw new Exception($"The collection with the name {newName} exist.");
            }
            await _database.RenameCollectionAsync(oldName, newName);
        }
        catch (Exception e)
        {  
            throw new Exception($"Failed to rename collection {oldName}. Error: {e.Message}");
        }
    }

    public IMongoCollection<TEntity> GetCollectionByName<TEntity>(string name)
    {
        try
        {
            return _database.GetCollection<TEntity>(name);
        }
        catch (Exception e)
        {
            throw new Exception($"The collection with the name {name} could not be found");
        }
    }

    public async Task<long> DocumentsCount<TEntity>(string collectionName)
    {
        try
        {
            if (!CheckCollectionExistence<TEntity>(collectionName))
            {
                throw new Exception($"The collection with the name {collectionName} does not exist.");
            }
            var collection = _database.GetCollection<TEntity>(collectionName);
            return await collection.CountDocumentsAsync(Builders<TEntity>.Filter.Empty);
        }
        catch (Exception e)
        {
            throw new Exception($"Something goes wrong with getting count of a collection '{collectionName}' documents, Error: {e.Message}");
        }
    }
    
    public async Task<long> EstimateDocumentsCount<TEntity>(string collectionName)
    {
        try
        {
            if (!CheckCollectionExistence<TEntity>(collectionName))
            {
                throw new Exception($"The collection with the name {collectionName} does not exist.");
            }
            var collection = _database.GetCollection<TEntity>(collectionName);
            return await collection.EstimatedDocumentCountAsync();
        }
        catch (Exception e)
        {
            throw new Exception($"Something goes wrong with getting count of a collection '{collectionName}' documents, Error: {e.Message}");
        }
    }

    private bool CheckCollectionExistence<TEntity>(string collectionName)
    {
        return _database.GetCollection<TEntity>(collectionName) != null;
    }
}