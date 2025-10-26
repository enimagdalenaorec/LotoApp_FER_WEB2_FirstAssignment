using LotoApp.Repositories;
using LotoApp.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LotoApp.Controllers
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [ApiController]
    //[Route("[controller]")]
    public class AdminController : ControllerBase
    {
        private readonly RoundService _roundService;
        private readonly IRoundRepository _roundRepository;

        public AdminController(RoundService roundService, IRoundRepository roundRepository)
        {
            _roundService = roundService;
            _roundRepository = roundRepository;
        }

        [HttpPost("new-round")]
        public async Task<IActionResult> NewRound()
        {
            await _roundService.StartNewRoundAsync();
            return NoContent();
        }

        [HttpPost("close")]
        public async Task<IActionResult> Close()
        {
            await _roundService.CloseCurrentRoundAsync();
            return NoContent();
        }

        public class StoreResultsRequest
        {
            public List<int> Numbers { get; set; } = new();
        }

        [HttpPost("store-results")]
        public async Task<IActionResult> StoreResults([FromBody] StoreResultsRequest request)
        {
            var lastRound = await _roundRepository.GetLastRoundAsync();

            if (lastRound == null)
                return BadRequest(new { error = "No rounds found" });

            if (lastRound.IsActive)
                return BadRequest(new { error = "Round is still active" });

            if (lastRound.DrawnNumbers == "")
                lastRound.DrawnNumbers = null;

            if (lastRound.DrawnNumbers != null)
                return BadRequest(new { error = "Numbers already drawn", drawnNumbers = lastRound.DrawnNumbers });
            var ok = await _roundService.StoreResultsAsync(request.Numbers);
            return ok ? NoContent() : BadRequest();
        }
    }
}
