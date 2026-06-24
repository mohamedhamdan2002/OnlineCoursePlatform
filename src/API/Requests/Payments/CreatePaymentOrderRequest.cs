namespace API.Requests.Payments;

public sealed record CreatePaymentOrderRequest(Guid CourseId);
public sealed record CapturePaymentOrderRequest(Guid PaymentId, string OrderId);