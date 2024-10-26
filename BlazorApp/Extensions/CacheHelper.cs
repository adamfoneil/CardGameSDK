using Microsoft.Extensions.Caching.Distributed;
using System.Text.Json;

namespace BlazorApp.Extensions;

internal static class CacheHelper
{
	public static async Task<T> GetOrAddAsync<T>(
		this IDistributedCache cache, string key, Func<Task<T>> factory, 
		TimeSpan expiresAfter)
	{
		var bytes = await cache.GetAsync(key);
		if (bytes != null)
		{
			return JsonSerializer.Deserialize<T>(bytes) ?? throw new InvalidOperationException("Could not deserialize cached value.");
		}
		
		T value = await factory();
		await cache.SetAsync(key, JsonSerializer.SerializeToUtf8Bytes(value), new DistributedCacheEntryOptions
		{
			AbsoluteExpirationRelativeToNow = expiresAfter			
		});
		
		return value;
	}
}
