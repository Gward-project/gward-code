using System.ComponentModel;

namespace Gwards.DAL.Enums;

public enum QuestType
{
    [Description("Задания на подписку")]
    Subscribe = 0,
    
    [Description("Задание на кошелек")]
    Wallet = 1,

    [Description("Задание на активности связанные с игрой")]
    GameAction = 2, //todo: need more analytics

    [Description("Задание на минт")]
    Mint = 3,
}
