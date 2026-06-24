using Domain.Common.Results;
using Microsoft.AspNetCore.Http;

namespace Application.Common.Interfaces;

public interface IPayPalService
{
    Task<Result<string>> CreateOrderAsync(decimal price);
    Task<Result> CaptureOrderAsync(string orderId);
    Task<bool> VerifyWebhookAsync(HttpRequest request, string body);

}