using System.Text;
using Gwards.Api.HostedServices;
using Gwards.Api.Models;
using Gwards.Api.Models.Configurations;
using Gwards.Api.Services;
using Gwards.Api.Services.Common;
using Gwards.Api.Services.Passport;
using Gwards.Api.Services.Telegram;
using Gwards.Api.Services.Ton;
using Gwards.Api.Utils;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using SteamWebAPI2.Utilities;

namespace Gwards.Api;

public static class DI
{
    public static void AddConfigurations(this IServiceCollection serviceCollection, IConfiguration configuration)
    {
        var jwtConfiguration = configuration.GetSection("JwtConfiguration").Get<JwtConfiguration>();
        var tonConfiguration = configuration.GetSection("Ton").Get<TonConfiguration>();
        var nftPassportConfiguration = configuration.GetSection("NftPassport").Get<NftPassportConfiguration>();
        var paymentsConfiguration = configuration.GetSection("Payments").Get<PaymentsConfiguration>();
        var commonRewardsConfiguration = configuration.GetSection("CommonRewards").Get<CommonRewardsConfiguration>();
        
        serviceCollection.AddSingleton(jwtConfiguration!);
        serviceCollection.AddSingleton(tonConfiguration!);
        serviceCollection.AddSingleton(nftPassportConfiguration!);
        serviceCollection.AddSingleton(paymentsConfiguration!);
        serviceCollection.AddSingleton(commonRewardsConfiguration!);
    }
    
    public static void AddHostedServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddHostedService<ScoringHostedService>();
        services.AddHostedService<TelegramSubscriptionsHostedService>();
        
        services.AddHostedService<GameQuestsHostedService>();
        services.AddSingleton(_ => new SteamWebInterfaceFactory(configuration.GetSection("SteamWebAPIKey").Value));

        services.AddHostedService<TransactionsProcessingHostedService>();
        services.AddSingleton(_ => new TransactionsQueue(100));
    }
    
    public static void AddServices(this IServiceCollection services)
    {
        services.AddScoped<MessageService>();
        services.AddScoped<TelegramAuthService>();
        services.AddScoped<TelegramReferralService>();
        services.AddScoped<UserService>();
        services.AddScoped<SteamService>();
        services.AddScoped<AuthUtils>();
        services.AddScoped<MetricsUtil>();
        services.AddScoped<QuestService>();
        services.AddScoped<TonService>();
        services.AddScoped<PaymentService>();
        services.AddScoped<NftMinterService>();
        services.AddScoped<NftMetadataGenerator>();
        services.AddScoped<FileStorageService>();
        services.AddScoped<PassportCoreService>();
        services.AddScoped<PassportExternalsService>();
        services.AddScoped<PassportPaymentsProcessor>();
        services.AddSingleton<ScoringService>();
    }
    
    public static void AddJwtAuthentication(this IServiceCollection services, IConfiguration configuration)
    {
        var jwtConfiguration = configuration.GetSection("JwtConfiguration").Get<JwtConfiguration>();
        services
            .AddAuthentication(opt =>
            {
                opt.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                opt.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(opts =>
            {
                byte[] signingKeyBytes = Encoding.UTF8.GetBytes(jwtConfiguration.Secret);

                opts.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = jwtConfiguration.Issuer,
                    ValidAudience = jwtConfiguration.Audience,

                    IssuerSigningKey = new SymmetricSecurityKey(signingKeyBytes)
                };
            })
            .AddCookie(_ => { })
            .AddSteam(_ => { });
    }
    
    public static void AddSwagger(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddSwaggerGen(c =>
        {
            c.EnableAnnotations();
            c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                Description = "JWT Authorization header using the Bearer scheme",
                Name = "Authorization",
                In = ParameterLocation.Header,
                Type = SecuritySchemeType.ApiKey,
                Scheme = "Bearer"
            });
            c.AddSecurityRequirement(new OpenApiSecurityRequirement()
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "Bearer"
                        },
                        Scheme = "oauth2",
                        Name = "Bearer",
                        In = ParameterLocation.Header,
                    },
                    new List<string>()
                }
            });
        });
    } 
}
