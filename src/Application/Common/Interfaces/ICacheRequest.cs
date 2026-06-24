using MediatR;

namespace Application.Common.Interfaces;

public interface ICacheRequest
{
    string CacheKey { get; }
    string[] Tags { get; }
    TimeSpan Expiration { get; }
}

public interface ICacheRequest<TResponse> : IRequest<TResponse>, ICacheRequest;
