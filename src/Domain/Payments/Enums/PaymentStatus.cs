namespace Domain.Payments.Enums;

public enum PaymentStatus
{
    Pending,      // بعد create order
    Processing,  // بعد capture
    Succeeded,   // بعد webhook
    Failed
}