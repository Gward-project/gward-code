using Gwards.Api.Services;
using Gwards.DAL;
using Microsoft.EntityFrameworkCore;

namespace Gwards.Api.HostedServices;

public class ScoringHostedService : BackgroundService
{
    private readonly ScoringService _scoringService;
    private readonly GwardsContext _dbContext;
    private readonly IServiceScope _scope;

    private const int ScoreCheckDelayInSeconds = 2 * 1000;
    private const int GlobalScoreCheckDelayInMs = 60 * 60 * 1000;
    
    public ScoringHostedService(IServiceProvider serviceProvider)
    {
        _scope = serviceProvider.CreateScope();
        _scoringService = _scope.ServiceProvider.GetRequiredService<ScoringService>();
        _dbContext = _scope.ServiceProvider.GetRequiredService<GwardsContext>();
    }
    
    protected override async Task ExecuteAsync(CancellationToken cancellationToken)
    {
        while (!cancellationToken.IsCancellationRequested)
        {
            var usersWithSteam = await _dbContext
                .Users
                .Include(x =>
                    x.ExternalAccounts.Where(y => y.Type == "steam")
                )
                .Where(x =>
                    x.ExternalAccounts.Any(y => y.Type == "steam")
                )
                .ToListAsync(cancellationToken);

            foreach (var user in usersWithSteam)
            {
                var steamAccount = user.ExternalAccounts.First(x => x.Type == "steam");
                var steamAccountId = ulong.Parse(steamAccount.AccountId);

                user.Score = await _scoringService.CalculateScore(steamAccountId);
                
                await Task.Delay(ScoreCheckDelayInSeconds, cancellationToken);
            }

            await _dbContext.SaveChangesAsync(cancellationToken);
            await Task.Delay(GlobalScoreCheckDelayInMs, cancellationToken);
        }
        
        _scope.Dispose();
        _scoringService.Dispose();
    }
}