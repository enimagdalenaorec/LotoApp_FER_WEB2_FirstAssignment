using LotoApp.Models;
using LotoApp.Repositories;
using LotoApp.Services;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace LotoApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IRoundRepository _roundRepository; // directly or via service
        private readonly TicketService _ticketService;

        public HomeController(ILogger<HomeController> logger, IRoundRepository roundRepository, TicketService ticketService)
        {
            _logger = logger;
            _roundRepository = roundRepository;
            _ticketService = ticketService;
        }

        public async Task<IActionResult> Index()
        {
            var activeRound = await _roundRepository.GetActiveRoundAsync();
            int ticketCount = 0;
            string? drawnNumbers = null;
            bool isActive = false;

            if (activeRound != null)
            {
                ticketCount = activeRound.Tickets.Count;
                isActive = activeRound.IsActive;
                drawnNumbers = activeRound.DrawnNumbers;
            }

            var model = new HomeViewModel
            {
                TicketCount = ticketCount,
                DrawnNumbers = drawnNumbers,
                IsRoundActive = isActive,
                UserEmail = User.Identity?.Name
            };

            return View(model);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
