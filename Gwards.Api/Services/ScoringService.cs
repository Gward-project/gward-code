using Gwards.Api.Constants;
using Gwards.Api.Utils;

namespace Gwards.Api.Services;

public class ScoringService : IDisposable
{
    private readonly SortedDictionary<double, double> _accountLifeMetric;
    private readonly SortedDictionary<double, double> _gamesAmountMetric;
    private readonly SortedDictionary<double, double> _friendsAmountMetric;
    private readonly SortedDictionary<double, double> _playTimeTotalMetric;
    private readonly SortedDictionary<double, double> _playTimeTwoWeeksMetric;

    private readonly IServiceScope _scope;

    public ScoringService(IServiceProvider serviceProvider)
    {
        _scope = serviceProvider.CreateScope();
        var metricsUtil = _scope.ServiceProvider.GetRequiredService<MetricsUtil>();
        
        _accountLifeMetric = metricsUtil.LoadMetric(MetricNames.AccountLifeTime);
        _gamesAmountMetric = metricsUtil.LoadMetric(MetricNames.GamesAmount);
        _friendsAmountMetric = metricsUtil.LoadMetric(MetricNames.FriendsAmount);
        _playTimeTotalMetric = metricsUtil.LoadMetric(MetricNames.PlayTimeTotal);
        _playTimeTwoWeeksMetric = metricsUtil.LoadMetric(MetricNames.PlayTimeTwoWeeks);
    }

    public async Task<int> CalculateScore(ulong steamAccountId)
    {
        var steamService = _scope.ServiceProvider.GetRequiredService<SteamService>();
        var scoringData = await steamService.GetScoringData(steamAccountId);
                
        var accountLifeScore = GetMetricValueByLowBoundKey(_accountLifeMetric, scoringData.AccountLifeTimeDays / 365.2425);
        var gamesAmountScore = GetMetricValueByLowBoundKey(_gamesAmountMetric, scoringData.GamesAmount);
        var friendsAmountScore = GetMetricValueByLowBoundKey(_friendsAmountMetric, scoringData.FriendsAmount);
        var playTimeTotalCoefficient = GetMetricValueByLowBoundKey(_playTimeTotalMetric, scoringData.PlayTimeTotalHours);
        var playTimeTwoWeeksCoefficient = GetMetricValueByLowBoundKey(_playTimeTwoWeeksMetric, scoringData.PlayTimeTwoWeeksHours);
        var avatarScore = scoringData.HasAvatar ? 1 : 0;

        var scores = accountLifeScore + gamesAmountScore + friendsAmountScore + avatarScore;
        var coefficients = playTimeTotalCoefficient + playTimeTwoWeeksCoefficient;
        var result = Math.Floor(scores * coefficients);

        if (result < 5)
        {
            result = 5;
        }
        
        if (result > 100)
        {
            result = 100;
        }
        
        return (int)result;
    }
    
    private static double GetMetricValueByLowBoundKey(SortedDictionary<double, double> metric, double searchKey)
    {
        var keys = metric.Keys.ToArray();
        var keyIndex = Array.BinarySearch(keys, searchKey);
        
        if (keyIndex < 0)
        {
            keyIndex = ~keyIndex - 1;
        }

        var key = keys[keyIndex];
        var value = metric[key];

        return value;
    }

    public void Dispose()
    {
        _scope.Dispose();
    }
}
