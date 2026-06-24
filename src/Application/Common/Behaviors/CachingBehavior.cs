using Application.Common.Interfaces;
using Domain.Common.Results;
using MediatR;
using Microsoft.Extensions.Caching.Hybrid;

namespace Application.Common.Behaviors;
public class CachingBehavior<TRequest, TResponse>(HybridCache cache) : IPipelineBehavior<TRequest, TResponse> where TRequest : notnull
{
    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        if (request is not ICacheRequest cachedRequest)
            return await next(cancellationToken);

        var result = await cache.GetOrCreateAsync(
            cachedRequest.CacheKey, 
            (cancellationToken) => new ValueTask<TResponse>((TResponse)(object)null!), 
            new HybridCacheEntryOptions
            {
                Flags = HybridCacheEntryFlags.DisableUnderlyingData
            }, 
            cancellationToken: cancellationToken
        );

        if(result is null)
        {
            result = await next(cancellationToken);
            if(result is Result requestResult && requestResult.IsSuccess)
            {
                await cache.SetAsync(
                    key: cachedRequest.CacheKey, 
                    value: result, 
                    options: new HybridCacheEntryOptions { Expiration = cachedRequest.Expiration },
                    tags: cachedRequest.Tags,
                    cancellationToken: cancellationToken
                );
            }
        }

        return result;
    }
}
