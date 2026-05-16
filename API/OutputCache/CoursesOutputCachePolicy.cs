using Application.Common.Interfaces;
using Microsoft.AspNetCore.OutputCaching;

namespace API.OutputCache;

public class CoursesOutputCachePolicy : IOutputCachePolicy
{
    public ValueTask CacheRequestAsync(OutputCacheContext context, CancellationToken cancellation)
    {
        var currentUser = context.HttpContext.RequestServices.GetRequiredService<ICurrentUser>();
        // This runs BEFORE cache lookup — sets the vary key
        context.CacheVaryByRules.VaryByValues.TryAdd(
            "user",
            currentUser.IsAuthenticated ? currentUser.UserId.ToString() : "anonymous"
        );

        context.EnableOutputCaching = true;
        context.AllowCacheLookup = true;      // ← allow reading from cache
        context.AllowCacheStorage = true;     // ← allow writing to cache
        context.AllowLocking = true;
        return ValueTask.CompletedTask;
    }

    public ValueTask ServeFromCacheAsync(OutputCacheContext context, CancellationToken ct)
         => ValueTask.CompletedTask;

    public ValueTask ServeResponseAsync(OutputCacheContext context, CancellationToken ct)
        => ValueTask.CompletedTask;
}
