using LotoApp.Models;
using LotoApp.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LotoApp.Controllers
{
    [Authorize]
    public class TicketController : Controller
    {
        private readonly TicketService _ticketService;
        private readonly AuthService _authService;

        public TicketController(TicketService ticketService, AuthService authService)
        {
            _ticketService = ticketService;
            _authService = authService;
        }

        // GET: /Ticket/Create
        [HttpGet("/Ticket/Create")]
        public IActionResult Create()
        {
            return View(new TicketCreateViewModel());
        }

        // POST: /Ticket/Create
        [HttpPost]
        public async Task<IActionResult> Create(TicketCreateViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var sub = User.FindFirst("sub")?.Value
                      ?? User.FindFirst("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier")?.Value
                      ?? User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(sub))
                return Unauthorized("User not authenticated properly.");

            var user = await _authService.GetOrCreateUserAsync(sub, User.FindFirst("email")?.Value);
            if (user == null)
                return BadRequest("Unable to find user.");

            var (qrBytes, error) = await _ticketService.CreateTicketAsync(model.DocumentNumber, model.SelectedNumbers, user.Id);

            if (error != null)
            {
                ModelState.AddModelError(string.Empty, error);
                return View(model);
            }

            return File(qrBytes!, "image/png");
        }

        // GET: /Ticket/{uuid}
        [AllowAnonymous]
        [HttpGet("/Ticket/{uuid}")]
        public async Task<IActionResult> Details(string uuid)
        {
            var ticket = await _ticketService.GetTicketByUuidAsync(uuid);
            if (ticket == null)
                return NotFound("Ticket not found.");

            var round = ticket.Round;

            var model = new
            {
                ticket.DocumentNumber,
                ticket.SelectedNumbers,
                DrawnNumbers = round?.DrawnNumbers,
                IsActive = round?.IsActive
            };

            return View("Details", model);
        }
    }
}
