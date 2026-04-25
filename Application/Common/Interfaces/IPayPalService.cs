using Domain.Common.Results;

namespace Application.Common.Interfaces;

public interface IPayPalService
{
    Task<Result<string>> CreateOrderAsync(decimal price);
    Task<Result> CaptureOrderAsync(string orderId);

}