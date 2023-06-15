using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using ScheduleBarbecue.Tests.Helpers.Api;
using ScheduleBarbecue.Infrastructure.Contexts;
using System.Security.Claims;

namespace ScheduleBarbecue.Tests.Helpers;

public class IntegrationTestBase<TStartup> where TStartup : class
{
    protected IServiceProvider? ServiceProvider;

    private WebApplicationFactory<TStartup>? _webApplicationFactory;
    private HttpClient? TestAppClient;

    public IntegrationTestBase(bool configureServer = true)
    {
        if (configureServer)
            ConfigureServer().GetAwaiter().GetResult();
    }

    protected async Task ConfigureServer()
    {
        _webApplicationFactory = new WebApplicationFactory<TStartup>()
            .WithWebHostBuilder(builder =>
            {
                builder.ConfigureTestServices(ConfigureServices);
            });

        TestAppClient = _webApplicationFactory.CreateClient();

        ServiceProvider = _webApplicationFactory.Server.Services;
    }

    protected HttpClient GetTestAppClient() => TestAppClient ?? new HttpClient();

    protected virtual void ConfigureServices(IServiceCollection services)
    {
        var root = new InMemoryDatabaseRoot();

        services.AddScoped(sp =>
        {
            return new DbContextOptionsBuilder<ScheduleBarbecueContext>()
            .UseInMemoryDatabase("InMemoryScheduleBarbecueTest", root)
            .UseApplicationServiceProvider(sp)
            .Options;
        });

        services.Configure<JwtBearerOptions>(JwtBearerDefaults.AuthenticationScheme, options =>
        {
            var config = new OpenIdConnectConfiguration()
            {
                Issuer = JwtMockHelper.Issuer
            };

            config.SigningKeys.Add(JwtMockHelper.SecurityKey);
            options.Configuration = config;
        });

        AfterConfigureServices(services);
    }

    protected virtual void AfterConfigureServices(IServiceCollection services)
    { }

    public void Dispose()
    {
        TestAppClient?.Dispose();
    }
}

public class IntegrationTestBaseWithFakeJWT<TStartup> : IntegrationTestBase<TStartup> where TStartup : class
{
    protected const string TEST_ID = "d0164e85-8bfc-4138-37e3-08d68bda781d";

    private readonly Action<MvcOptions>? _setupAction;

    protected readonly FakeUserHelper DefaultFakeUserFilter = new(
        new List<Claim>()
        {
                new Claim("uid", TEST_ID),
                new Claim(ClaimTypes.Name, "Teste"),
                new Claim(ClaimTypes.Email, "test@teste.com"),
        }
    );

    public IntegrationTestBaseWithFakeJWT(bool configureServer = true, Action<MvcOptions>? setupAction = null)
        : base(configureServer && setupAction == null)
    {
        _setupAction = setupAction;

        if (configureServer && setupAction != null)
            ConfigureServer().GetAwaiter().GetResult();
    }

    protected override void AfterConfigureServices(IServiceCollection services)
    {
        base.AfterConfigureServices(services);

        // mocked user authentication with fake JWT
        services.AddSingleton<IAuthorizationHandler, AllowAnonymous>();
        services.AddMvc(_setupAction ?? (options =>
        {
            options.Filters.Add(new AllowAnonymousFilter());
            options.Filters.Add(DefaultFakeUserFilter);
        }));
    }

    public class AllowAnonymous : IAuthorizationHandler
    {
        public Task HandleAsync(AuthorizationHandlerContext context)
        {
            foreach (IAuthorizationRequirement requirement in context.PendingRequirements.ToList())
                context.Succeed(requirement); //Simply pass all requirements

            return Task.CompletedTask;
        }
    }
}