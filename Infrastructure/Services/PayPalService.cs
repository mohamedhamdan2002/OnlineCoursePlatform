
using Application.Common.Interfaces;
using Application.Common.Settings;
using Domain.Common.Results;
using Microsoft.Extensions.Options;

namespace Infrastructure.Services;
public sealed class PayPalService(HttpClient httpClient, IOptions<PayPalSettings> options) : IPayPalService
{
    private readonly HttpClient _httpClient = httpClient;
    private readonly PayPalSettings _settings = options.Value;

    private Task GetAccessToken()
    {
        throw new NotImplementedException();        
    }

   
    public Task<Result<string>> CreateOrderAsync(decimal price)
    {
        throw new NotImplementedException();
    }

    public Task<Result> CaptureOrderAsync(string orderId)
    {
        throw new NotImplementedException();
    }
}
