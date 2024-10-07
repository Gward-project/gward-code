using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Gwards.DAL;

public static class DI
{
    public static void AddDatabase(this IServiceCollection services, IConfiguration cfg)
    {
        var connectionString = cfg.GetSection("ConnectionString").Value;

        services.AddDbContext<GwardsContext>(opt =>
            opt.UseNpgsql(connectionString, b => b.MigrationsAssembly(typeof(GwardsContext).Assembly.FullName)));
    }

    public static async Task MigrateDatabase(this IServiceProvider serviceProvider)
    {
        using var scope = serviceProvider.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<GwardsContext>();

        await db.Database.MigrateAsync();
    }
}
