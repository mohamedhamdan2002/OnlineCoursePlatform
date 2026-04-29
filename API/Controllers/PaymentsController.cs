using API.Requests.Payments;
using Application.Common.Interfaces;
using Application.Features.Payments.Commands.CapturePaymentOrder;
using Application.Features.Payments.Commands.ConfirmPayment;
using Application.Features.Payments.Commands.CreatePaymentOrder;
using Application.Features.Payments.Dots;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace API.Controllers;

[Authorize]
public class PaymentsController(ISender sender, IPayPalService payPal) : BaseApiController
{
    [HttpPost("create")]
    public async Task<ActionResult<PaymentOrderDto>> CreatePaymentOrder(CreatePaymentOrderRequest request, CancellationToken  cancellationToken)
    {
        var command = new CreatePaymentOrderCommand(request.CourseId);
        var result = await sender.Send(command, cancellationToken);
        return HandleResult(result, () => Ok(result.Data));
    }

    [HttpPost("capture")]
    public async Task<IActionResult> CapturePaymentOrder(CapturePaymentOrderRequest request, CancellationToken cancellationToken)
    {
        var command = new CapturePaymentOrderCommand(request.OrderId, request.PaymentId);
        var result = await sender.Send(command, cancellationToken);
        return HandleResult(result, () => Ok());
    }

    [HttpPost("paypal/webhook")]
    [AllowAnonymous]
    public async Task<IActionResult> HandleWebHook(CancellationToken cancellationToken)
    {

        var body = await new StreamReader(Request.Body).ReadToEndAsync();

        var isValid = await payPal.VerifyWebhookAsync(Request, body);

        if (!isValid)
            return Unauthorized();

        var json = JsonDocument.Parse(body);

        var eventType = json.RootElement.GetProperty("event_type").GetString();

        if (eventType == "PAYMENT.CAPTURE.COMPLETED")
        {
            var orderId = json.RootElement
                .GetProperty("resource")
                .GetProperty("supplementary_data")
                .GetProperty("related_ids")
                .GetProperty("order_id")
                .GetString();

            await sender.Send(new ConfirmPaymentCommand(orderId!));
        }

        return Ok();
    }

}
