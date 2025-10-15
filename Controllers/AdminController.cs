using LotoApp.Services;
using Microsoft.AspNetCore.Mvc;

namespace LotoApp.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AdminController : ControllerBase
    {
        private readonly RoundService _roundService;

        public AdminController(RoundService roundService)
        {
            _roundService = roundService;
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
            var ok = await _roundService.StoreResultsAsync(request.Numbers);
            return ok ? NoContent() : BadRequest();
        }
    }
}
