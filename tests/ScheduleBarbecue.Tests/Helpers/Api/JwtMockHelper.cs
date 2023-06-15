using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace ScheduleBarbecue.Tests.Helpers.Api;

public static class JwtMockHelper
{
    public static string Issuer { get; }
    public static string Audience { get; }
    public static SecurityKey SecurityKey { get; }
    public static SigningCredentials SigningCredentials { get; }

    static JwtMockHelper()
    {
        var configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: false)
            .AddEnvironmentVariables()
            .Build();

        configuration["JWT:Secret"] = "fake-secret";

        SecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JWT:Secret"])) { KeyId = Guid.NewGuid().ToString() };
        SigningCredentials = new SigningCredentials(SecurityKey, SecurityAlgorithms.HmacSha256Signature);
    }
}