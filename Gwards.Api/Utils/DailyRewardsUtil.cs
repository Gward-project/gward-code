namespace Gwards.Api.Utils;

public static class DailyRewardsUtil
{
    public static int GetNextDay(int rewardDay)
    {
        if (rewardDay == 7)
        {
            return 1;
        }

        return rewardDay + 1;
    }

    public static int GetReward(int rewardDay)
    {
        return rewardDay switch
        {
            1 => 10,
            2 => 20,
            3 => 30,
            4 => 40,
            5 => 50,
            6 => 60,
            7 => 100,
            _ => throw new ArgumentException("Invalid reward day was provided")
        };
    }
}