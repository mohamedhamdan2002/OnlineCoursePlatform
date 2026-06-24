namespace Application.Common.Settings;

public class JwtSettings
{

    public string ValidIssuer { get; set; } = null!;
    public string ValidAudience { get; set; } = null!;
    public string SecretKey { get; set; } = null!;
    public double ExpireOn { get; set; }
  
}
