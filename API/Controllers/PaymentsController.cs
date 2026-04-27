using API.Requests.Payments;
using Application.Common.Interfaces;
using Application.Features.Payments.Commands.CapturePaymentOrder;
using Application.Features.Payments.Commands.ConfirmPayment;
using Application.Features.Payments.Commands.CreatePaymentOrder;
using Application.Features.Payments.Dots;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace API.Controllers;

public class PaymentsController(ISender sender, IPayPalService payPal) : BaseApiController
{
    [HttpPost("create")]
    public async Task<ActionResult<PaymentOrderDto>> CreatePaymentOrder(CreatePaymentOrderRequest request, CancellationToken  cancellationToken)
    {
        // just for test 
        var userId = new Guid("5EE291B3-BFE8-4ECC-9C50-08DE7E1AF89E");
        var command = new CreatePaymentOrderCommand(userId, request.CourseId);
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

    [HttpPost("paypal/webhook")]
    public async Task<IActionResult> HandleWebHook(CancellationToken cancellationToken)
    {

        await Task.Delay(2000); // for test
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
