using API.Requests.Payments;
using Application.Features.Payments.Commands.CapturePaymentOrder;
using Application.Features.Payments.Commands.CreatePaymentOrder;
using Application.Features.Payments.Dots;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

public class PaymentsController(ISender sender) : BaseApiController
{
    [HttpPost("create")]
    public async Task<ActionResult<PaymentOrderDto>> CreatePaymentOrder(CreatePaymentOrderRequest request, CancellationToken  cancellationToken)
    {
        var command = new CreatePaymentOrderCommand(request.UserId, request.CourseId);
        var result = await sender.Send(command, cancellationToken);
        return HandleResult(result, () => Ok(result.Data));
    }

    [HttpPost("capture")]
    public async Task<ActionResult<PaymentOrderDto>> CapturePaymentOrder(CapturePaymentOrderRequest request, CancellationToken cancellationToken)
    {
        var command = new CapturePaymentOrderCommand(request.OrderId, request.PaymentId);
        var result = await sender.Send(command, cancellationToken);
        return HandleResult(result, () => Ok());
    }
}
