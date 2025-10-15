using LotoApp.Data;
using LotoApp.Models;
using Microsoft.EntityFrameworkCore;

namespace LotoApp.Repositories
{
    public class TicketRepository : ITicketRepository
    {
        private readonly AppDbContext _context;

        public TicketRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Ticket?> GetByUuidAsync(string uuid)
        {
            return await _context.Tickets
                .Include(t => t.Round)
                .FirstOrDefaultAsync(t => t.UUID == uuid);
        }

        public async Task AddAsync(Ticket ticket)
        {
            await _context.Tickets.AddAsync(ticket);
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }

        public async Task<Round?> GetActiveRoundAsync()
        {
            return await _context.Rounds.FirstOrDefaultAsync(r => r.IsActive);
        }
    }
}
