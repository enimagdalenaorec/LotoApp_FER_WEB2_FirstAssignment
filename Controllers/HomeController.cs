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
        private readonly IRoundRepository _roundRepository;
        private readonly ITicketRepository _ticketRepository;
        private readonly AuthService _authService;

        public HomeController(
            ILogger<HomeController> logger,
            IRoundRepository roundRepository,
            ITicketRepository ticketRepository,
            AuthService authService)
        {
            _logger = logger;
            _roundRepository = roundRepository;
            _ticketRepository = ticketRepository;
            _authService = authService;
        }

        public async Task<IActionResult> Index()
        {
            // var activeRound represents the round that is open or the round that is closed but whose numbers arnt drawn yet
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
            // if there are no active rounds, show the last round info
            else
            {
                var lastRound = await _roundRepository.GetLastRoundAsync();
                if (lastRound != null)
                {
                    ticketCount = lastRound.Tickets.Count;
                    drawnNumbers = lastRound.DrawnNumbers;
                    isActive = lastRound.IsActive;
                }
            }

            var model = new HomeViewModel
            {
                TicketCount = ticketCount,
                DrawnNumbers = drawnNumbers,
                IsRoundActive = isActive,
                UserEmail = User.Identity?.Name
            };

            // if user is logged in, load their tickets
            if (User.Identity?.IsAuthenticated == true)
            {
                var sub = User.FindFirst("sub")?.Value
                          ?? User.FindFirst("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier")?.Value
                          ?? User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;

                if (!string.IsNullOrEmpty(sub))
                {
                    var user = await _authService.GetOrCreateUserAsync(sub, User.FindFirst("email")?.Value);
                    if (user != null)
                    {
                        // load all tickets for this user
                        var userTickets = await _ticketRepository
                            .GetTicketsByUserIdAsync(user.Id);

                        // show user's tickets only of the active round (whose numbers arent yet drawn)
                        if (activeRound != null)
                        {
                            userTickets = userTickets
                                .Where(t => t.RoundId == activeRound.Id)
                                .ToList();
                        }
                        // if no active round, show the latest one
                        else if (userTickets.Any())
                        {
                            var lastRound = await _roundRepository.GetLastRoundAsync();
                            if (lastRound != null)
                            {
                                userTickets = userTickets
                                    .Where(t => t.RoundId == lastRound.Id)
                                    .ToList();
                            }
                            else
                            {
                                userTickets = Enumerable.Empty<Ticket>();
                            }
                        }

                        model.UserTickets = userTickets.ToList();
                    }
                }
            }

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
