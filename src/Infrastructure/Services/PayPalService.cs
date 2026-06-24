using Application.Common.Interfaces;
using Application.Common.Settings;
using Domain.Common.Results;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

public sealed class PayPalService(HttpClient httpClient, IOptions<PayPalSettings> options) : IPayPalService
{
    private readonly HttpClient _httpClient = httpClient;
    private readonly PayPalSettings _settings = options.Value;

    private async Task<string> GetAccessToken()
    {
        var authToken = Convert.ToBase64String(
            Encoding.UTF8.GetBytes($"{_settings.ClientID}:{_settings.ClientSecret}")
        );

        var request = new HttpRequestMessage(HttpMethod.Post, $"{_settings.BaseUrl}/v1/oauth2/token");

        request.Headers.Authorization = new AuthenticationHeaderValue("Basic", authToken);

        request.Content = new FormUrlEncodedContent(new Dictionary<string, string>
        {
            { "grant_type", "client_credentials" }
        });

        var response = await _httpClient.SendAsync(request);

        if (!response.IsSuccessStatusCode)
            throw new Exception("Failed to get PayPal access token");

        var content = await response.Content.ReadAsStringAsync();

        using var doc = JsonDocument.Parse(content);
        return doc.RootElement.GetProperty("access_token").GetString()!;
    }

    public async Task<Result<string>> CreateOrderAsync(decimal price)
    {
        try
        {
            var token = await GetAccessToken();

            var request = new HttpRequestMessage(HttpMethod.Post, $"{_settings.BaseUrl}/v2/checkout/orders");

            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var body = new
            {
                intent = "CAPTURE",
                purchase_units = new[]
                {
                    new
                    {
                        amount = new
                        {
                            currency_code = "USD",
                            value = price.ToString("F2")
                        }
                    }
                }
            };

            request.Content = new StringContent(
                JsonSerializer.Serialize(body),
                Encoding.UTF8,
                "application/json"
            );

            var response = await _httpClient.SendAsync(request);

            if (!response.IsSuccessStatusCode)
                return Result.Fail<string>(new Error(400, "Failed to create PayPal order"));

            var content = await response.Content.ReadAsStringAsync();

            using var doc = JsonDocument.Parse(content);

            var orderId = doc.RootElement.GetProperty("id").GetString();

            return Result.Success(orderId!);
        }
        catch (Exception ex)
        {
            return Result.Fail<string>(new Error(400, ex.Message));
        }
    }

    public async Task<Result> CaptureOrderAsync(string orderId)
    {
        try
        {
            var token = await GetAccessToken();

            var request = new HttpRequestMessage(
                HttpMethod.Post,
                $"{_settings.BaseUrl}/v2/checkout/orders/{orderId}/capture"
            );

            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);

            request.Content = new StringContent("", Encoding.UTF8, "application/json");

            var response = await _httpClient.SendAsync(request);

            if (!response.IsSuccessStatusCode)
                return Result.Fail<string>(new Error(400, "Failed to create PayPal order"));

            var content = await response.Content.ReadAsStringAsync();

            using var doc = JsonDocument.Parse(content);

            var status = doc.RootElement.GetProperty("status").GetString();

            if (status != "COMPLETED")
                return Result.Fail<string>(new Error(400, "Payment not completed"));

            return Result.Success();
        }
        catch (Exception ex)
        {
            return Result.Fail<string>(new Error(400, ex.Message));
        }
    }

    public async Task<bool> VerifyWebhookAsync(HttpRequest request, string body)
    {
        try
        {
            // 1️⃣ Extract headers
            var transmissionId = request.Headers["PayPal-Transmission-Id"].ToString();
            var transmissionTime = request.Headers["PayPal-Transmission-Time"].ToString();
            var certUrl = request.Headers["PayPal-Cert-Url"].ToString();
            var authAlgo = request.Headers["PayPal-Auth-Algo"].ToString();
            var transmissionSig = request.Headers["PayPal-Transmission-Sig"].ToString();

            if (string.IsNullOrWhiteSpace(transmissionId))
                return false;

            // 2️⃣ Get access token
            var token = await GetAccessToken();

            // 3️⃣ Build verification request
            var verifyRequest = new PayPalVerifyRequest
            {
                transmission_id = transmissionId,
                transmission_time = transmissionTime,
                cert_url = certUrl,
                auth_algo = authAlgo,
                transmission_sig = transmissionSig,
                webhook_id = _settings.WebhookId,
                webhook_event = JsonSerializer.Deserialize<object>(body)!
            };

            var httpRequest = new HttpRequestMessage(
                HttpMethod.Post,
                $"{_settings.BaseUrl}/v1/notifications/verify-webhook-signature"
            );

            httpRequest.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);

            httpRequest.Content = new StringContent(
                JsonSerializer.Serialize(verifyRequest),
                Encoding.UTF8,
                "application/json"
            );

            var response = await _httpClient.SendAsync(httpRequest);

            if (!response.IsSuccessStatusCode)
                return false;

            var content = await response.Content.ReadAsStringAsync();

            var result = JsonSerializer.Deserialize<PayPalVerifyResponse>(content);

            return result?.verification_status == "SUCCESS";
        }
        catch
        {
            return false;
        }
    }
}

public class PayPalVerifyRequest
{
    public string transmission_id { get; set; } = default!;
    public string transmission_time { get; set; } = default!;
    public string cert_url { get; set; } = default!;
    public string auth_algo { get; set; } = default!;
    public string transmission_sig { get; set; } = default!;
    public string webhook_id { get; set; } = default!;
    public object webhook_event { get; set; } = default!;
}

public class PayPalVerifyResponse
{
    public string verification_status { get; set; } = default!;
}