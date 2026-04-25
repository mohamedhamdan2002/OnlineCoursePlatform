using Domain.Common.Results;

namespace Domain.Payments;
public static class PaymentErrors
{
    public static Error IdRequired = new Error(400, "Payment Id is Required");

    public static Error UserIdRequired = new Error(400, "UserId is Required");

    public static Error CourseIdRequired = new Error(400, "CourseId is Required");

    public static Error InvalidAmount = new Error(400, "Payment amount must be greater than 0");

    public static Error ProviderPaymentIdRequired = new Error(400, "Provider Payment Id is Required");

    public static Error ProviderPaymentIdAlreadySet = new Error(400, "Provider Payment Id is already set");

    public static Error InvalidStateTransition = new Error(400, "Invalid payment state transition");
}