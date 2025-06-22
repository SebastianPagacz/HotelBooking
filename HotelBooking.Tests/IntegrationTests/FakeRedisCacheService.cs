using System.Collections.Concurrent;
using HotelBooking.Application.Services;

namespace HotelBooking.Tests.IntegrationTests;

public class FakeRedisCacheService : IRedisCacheService
{
    private readonly ConcurrentDictionary<string, object> _store = new();

    public Task<T> GetAsync<T>(string key)
    {
        if (_store.TryGetValue(key, out var value))
            return Task.FromResult((T)value);
        return Task.FromResult(default(T));
    }

    public Task RemoveAsync(string key)
    {
        _store.TryRemove(key, out _);
        return Task.CompletedTask;
    }

    public Task SetAsync<T>(string key, T value, TimeSpan expiry)
    {
        _store[key] = value!;
        return Task.CompletedTask;
    }
}