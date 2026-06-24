namespace Application.Common.Settings;

public class PayPalSettings
{

    public string ClientID { get; set; } = null!;
    public string ClientSecret { get; set; } = null!;
    public string BaseUrl { get; set; } = null!;
    public string WebhookId { get; set; } = null!;
}
