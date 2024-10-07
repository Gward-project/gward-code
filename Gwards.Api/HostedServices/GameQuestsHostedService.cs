using Gwards.DAL;
using Gwards.DAL.Entities.Quests;
using Gwards.DAL.Enums;
using Microsoft.EntityFrameworkCore;
using SteamWebAPI2.Interfaces;
using SteamWebAPI2.Utilities;

namespace Gwards.Api.HostedServices
{
    public class GameQuestsHostedService : BackgroundService
    {
        private readonly GwardsContext _gwardsContext;
        private readonly PlayerService _steamUserStats;
        private readonly IServiceScope _scope;
        
        private const int QuestCheckDelayInMs = 5 * 60 * 1000;

        public GameQuestsHostedService(IServiceProvider serviceProvider)
        {
            _scope = serviceProvider.CreateScope();

            _gwardsContext = _scope.ServiceProvider.GetRequiredService<GwardsContext>();

            var httpClientFactory = _scope.ServiceProvider.GetRequiredService<IHttpClientFactory>();
            var steamFactory = _scope.ServiceProvider.GetRequiredService<SteamWebInterfaceFactory>();
            var httpClient = httpClientFactory.CreateClient();
            
            _steamUserStats = steamFactory.CreateSteamWebInterface<PlayerService>(httpClient);
        }

        protected override async Task ExecuteAsync(CancellationToken cancellationToken)
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                var usersWithQuests = await _gwardsContext
                    .Users
                    .Include(x => 
                        x.ExternalAccounts.Where(y => y.Type == "steam")
                    )
                    .Include(x =>
                        x.UserQuests.Where(y => y.Status == QuestStatus.New && y.Quest.Type == QuestType.GameAction)
                    )
                    .ThenInclude(x => x.Quest)
                    .Where(x =>
                        x.UserQuests.Any(y => y.Status == QuestStatus.New && y.Quest.Type == QuestType.GameAction)
                    )
                    .Where(x =>
                        x.ExternalAccounts.Any(y => y.Type == "steam")
                    )
                    .ToListAsync(cancellationToken);

                foreach (var user in usersWithQuests)
                {
                    var steamAccount = user.ExternalAccounts.First(x => x.Type == "steam");
                    var steamAccountId = steamAccount.AccountId;
                    var gamesData = await _steamUserStats.GetOwnedGamesAsync(ulong.Parse(steamAccountId), includeFreeGames: true);

                    foreach (var quest in user.UserQuests)
                    {
                        var gameQuest = (GameQuestEntity)quest.Quest;
                        var appId = gameQuest.ApplicationId;
                        
                        var userGameData = gamesData.Data.OwnedGames.FirstOrDefault(x => x.AppId == appId);
                        if (userGameData is not { PlaytimeForever.Minutes: > 0 })
                        {
                            continue;
                        }
                        
                        quest.Status = QuestStatus.ReadyForClaim;
                    }
                }
                
                await _gwardsContext.SaveChangesAsync(cancellationToken);
                await Task.Delay(QuestCheckDelayInMs, cancellationToken);
            }

            _scope.Dispose();
        }
    }
}
