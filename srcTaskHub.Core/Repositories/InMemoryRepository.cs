
using System.Collections.Concurrent;
using TaskHubCore.Interfaces;
using Microsoft.Extensions.Logging;
using System.Threading.Channels;
using System.Linq;

namespace TaskHubCore.Core.Repositories;

// need to specify what TEntity is becuase it can be anything until that is given 
public class InMemoryRepository<TKey, TEntity> : IRepositoryEntity<TKey, TEntity> where TEntity : IEntity<TKey>
{
    private readonly ILogger<InMemoryRepository<TKey, TEntity>> _logger;
    private ConcurrentDictionary<TKey, TEntity> _cache = new();
    private readonly Channel<TEntity> _channel = Channel.CreateUnbounded<TEntity>();
    public InMemoryRepository(ILogger<InMemoryRepository<TKey, TEntity>> logger)
    {
        _logger = logger;
    }
    public async ValueTask AddAsync(TKey key, TEntity entity)
    {
        if(_cache is null) {
            _logger.LogInformation("Cache is empty and null, creating a new dictionary");
            _cache = new();
        }
        try{
            var EntityAdded = _cache.TryAdd(key, entity);
            if(!EntityAdded) _logger.LogInformation($"Entity addition to cache {key} failed.");
            await _channel.Writer.WriteAsync(entity);
        }
        catch(Exception ex){
            // log the exception 
            _logger.LogError(ex, "Unable to add a new entry");
        }
    }

    public async ValueTask<bool?> DeleteAllAsync()
    {
        if(_cache is null) {
            _logger.LogInformation("Cache is empty and null, creating a new dictionary");
            throw new ArgumentNullException(nameof(_cache));
        }
        try{
            _cache.Clear();
            return true;
        }
        catch(Exception ex){
            _logger.LogError(ex, "Unable to empty the cache");
            return false;
        }
    }

    public async ValueTask<bool> DeleteAsync(TKey key)
    {
         if(_cache is null) {
            _logger.LogInformation("Cache is empty and null");
            throw new ArgumentNullException(nameof(_cache));
        }
        try {
            var deletedEntry = _cache.TryRemove(key, out var outparam);
            _logger.LogInformation( deletedEntry.ToString() + "" + outparam?.ToString());
            return true;
        }
        catch(Exception ex){
            _logger.LogError(ex, "Unable to empty the cache");
            return false;
        }

    }

    public ValueTask<TEntity?> GetAsync(TKey key)
    {
        if(_cache is null) {
            _logger.LogInformation("Cache is empty and null");
            throw new ArgumentNullException(nameof(_cache));
        }
        try{
            _cache.TryGetValue(key, out var outparam);
            return new ValueTask<TEntity?>(outparam);
        }
        catch(Exception ex){
             _logger.LogError(ex, "Unable to get the data");
            return new ValueTask<TEntity?>(null);
        }
    }

    public ValueTask<bool> UpdateAsync(TKey key, TEntity entity)
    {
        if(_cache is null) {
            _logger.LogInformation("Cache is empty and null");
            throw new ArgumentNullException(nameof(_cache));
        }
        try{
            _cache.TryGetValue(key, out var outparam);
            if(outparam is null) throw new ArgumentException("Record not found to update");
            var updateEntity = _cache.TryUpdate(key, entity, outparam);
            _channel.Writer.WriteAsync(entity);
            return new ValueTask<bool>(updateEntity);
        }
        catch(Exception ex){
             _logger.LogError(ex, "Unable to get the data");
            return new ValueTask<bool>(false);
        }
    }

    public async IAsyncEnumerable<TEntity> GetAllAsync()
    {
        foreach(var entity in _cache.Values){
            yield return entity;
        }
        await foreach(var item in _channel.Reader.ReadAllAsync()){
            yield return item;
        }
    }
}