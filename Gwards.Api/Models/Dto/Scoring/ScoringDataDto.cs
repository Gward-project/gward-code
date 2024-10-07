namespace Gwards.Api.Models.Dto.Scoring;

public class ScoringDataDto
{
    public double AccountLifeTimeDays { get; set; }
    
    public long FriendsAmount { get; set; }
    public long GamesAmount { get; set; }
    public bool HasAvatar { get; set; }
    
    public double PlayTimeTotalHours { get; set; }
    public double PlayTimeTwoWeeksHours { get; set; }
}