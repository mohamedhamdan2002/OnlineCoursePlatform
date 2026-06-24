using Domain.Common;
using Domain.Common.Results;
using Domain.Payments.Enums;

namespace Domain.Payments;

public sealed class Payment : Entity
{
    public Guid UserId { get; private set; }
    public Guid CourseId { get; private set; }

    public decimal Amount { get; private set; }
    public PaymentStatus Status { get; private set; }

    public string? OrderId { get; private set; }
    public PaymentProvider Provider { get; private set; }

    private Payment() { }

    private Payment(
        Guid id,
        Guid userId,
        Guid courseId,
        decimal amount,
        PaymentProvider provider
    ) : base(id)
    {
        UserId = userId;
        CourseId = courseId;
        Amount = amount;
        Provider = provider;
        Status = PaymentStatus.Pending;
    }

    public static Result<Payment> Create(
        Guid id,
        Guid userId,
        Guid courseId,
        decimal amount,
        PaymentProvider provider)
    {
        if (id == Guid.Empty)
            return Result.Fail<Payment>(PaymentErrors.IdRequired);

        if (userId == Guid.Empty)
            return Result.Fail<Payment>(PaymentErrors.UserIdRequired);

        if (courseId == Guid.Empty)
            return Result.Fail<Payment>(PaymentErrors.CourseIdRequired);

        if (amount <= 0)
            return Result.Fail<Payment>(PaymentErrors.InvalidAmount);

        return Result.Success(new Payment(
            id,
            userId,
            courseId,
            amount,
            provider
        ));
    }

    public Result SetOrderId(string orderId)
    {
        if (Status != PaymentStatus.Pending)
            return Result.Fail(PaymentErrors.InvalidStateTransition);

        if (!string.IsNullOrWhiteSpace(OrderId))
            return Result.Fail(PaymentErrors.OrderIdAlreadySet);

        if (string.IsNullOrWhiteSpace(orderId))
            return Result.Fail(PaymentErrors.OrderIdRequired);

        OrderId = orderId;

        return Result.Success();
    }

    public Result MarkAsSucceeded()
    {
        if (Status != PaymentStatus.Processing)
            return Result.Fail(PaymentErrors.InvalidStateTransition);

        if (string.IsNullOrWhiteSpace(OrderId))
            return Result.Fail(PaymentErrors.OrderIdRequired);

        Status = PaymentStatus.Succeeded;

        return Result.Success();
    }

    public Result MarkAsProcessing()
    {
        if (Status != PaymentStatus.Pending)
            return Result.Fail(PaymentErrors.InvalidStateTransition);

        if (string.IsNullOrWhiteSpace(OrderId))
            return Result.Fail(PaymentErrors.OrderIdRequired);

        Status = PaymentStatus.Processing;

        return Result.Success();
    }

    public Result MarkAsFailed()
    {
        if (Status != PaymentStatus.Pending)
            return Result.Fail(PaymentErrors.InvalidStateTransition);

        Status = PaymentStatus.Failed;

        return Result.Success();
    }
}
