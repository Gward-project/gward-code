using Gwards.Api.MIddlewares;
using Gwards.Api.Models;
using Gwards.Api.Services;
using Gwards.Api.Services.Telegram;
using Gwards.DAL;
using Telegram.Bot;

namespace Gwards.Api;

public class Program
{
    public static async Task Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        
        builder.Services.AddConfigurations(builder.Configuration);
        builder.Services.AddControllers().AddNewtonsoftJson();
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddHttpClient();
        builder.Services.AddSwagger();
        builder.Services.AddJwtAuthentication(builder.Configuration);
        
        builder.Services.AddServices();
        builder.Services.AddHostedServices(builder.Configuration);
        builder.Services.AddDatabase(builder.Configuration);
        
        AddTelegramClient(builder.Services, builder.Configuration);

        var app = builder.Build();

        if (!app.Environment.IsProduction())
        {
            app.UseSwagger(c =>
            {
                c.RouteTemplate = "api/swagger/{documentName}/swagger.json";
            });
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("v1/swagger.json", "V1");
                c.RoutePrefix = "api/swagger";
            });
        }

        app.UseCors(x => x.AllowAnyHeader().AllowAnyMethod().AllowCredentials().SetIsOriginAllowed(_ => true));
        app.UseHttpsRedirection();
        app.UseAuthentication();
        app.UseAuthorization();
        app.UseStaticFiles("/api/static");
        app.UseMiddleware<ErrorHandlerMiddleware>();

        app.MapControllers();

        await app.Services.MigrateDatabase();

        app.Run();
    }

    private static void AddTelegramClient(IServiceCollection services, IConfiguration cfg)
    {
        var telegramBotConfig = cfg.GetSection("TelegramBot").Get<TelegramBotConfig>();

        services.AddSingleton<ITelegramBotClient>(_ => new TelegramBotClient(telegramBotConfig.BotApiKey));
        services.AddHostedService<ConfigureWebhook>();
        services.AddSingleton(telegramBotConfig);
        services.AddScoped<MessageService>();
    }
}
