using Gwards.Api.Models.Dto.Nft;
using Gwards.Api.Models.Dto.Quests;
using Gwards.Api.Models.Dto.Responses;
using Gwards.Api.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Gwards.Api.Controllers;

[Authorize]
public class QuestsController : BaseController
{
    private readonly QuestService _questService;

    public QuestsController(QuestService questService)
    {
        _questService = questService;
    }

    [HttpGet]
    public async Task<ApiResponse<List<QuestInfoDto>>> GetActive()
    {
        var userId = ExtractUserId();
        var response = await _questService.GetUserActiveQuests(userId);

        return Ok(response);
    }
    
    [HttpGet("rewards")]
    public async Task<ApiResponse<ICollection<NftRewardDto>>> GetRewards()
    {
        var userId = ExtractUserId();
        var response = await _questService.GetUserQuestRewards(userId);

        return Ok(response);
    }

    [HttpPost("{questId}/claim")]
    public async Task<IActionResult> Claim([FromRoute] int questId)
    {
        var userId = ExtractUserId();
        await _questService.Claim(questId, userId);

        return Ok();
    }
}
