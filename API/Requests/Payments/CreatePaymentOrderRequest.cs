namespace API.Requests.Payments;

public sealed record CreatePaymentOrderRequest(Guid CourseId, Guid UserId);
public sealed record CapturePaymentOrderRequest(Guid PaymentId, string OrderId);