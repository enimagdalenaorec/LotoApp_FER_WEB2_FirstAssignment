using LotoApp.Data;
using LotoApp.Models;
using Microsoft.EntityFrameworkCore;

namespace LotoApp.Repositories
{
    public class RoundRepository : IRoundRepository
    {
        private readonly AppDbContext _context;

        public RoundRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Round?> GetActiveRoundAsync()
        {
            // var activeRound represents the round that is open or the round that is closed but whose numbers arnt drawn yet
            var activeRound = await _context.Rounds
                .Include(r => r.Tickets)
                .FirstOrDefaultAsync(r => r.IsActive);
            if (activeRound != null)
            {
                return activeRound;
            } else
            {
                // try to reach the one that isnt active but hasnt got drawn numbers yet
                activeRound = await _context.Rounds
                    .Include(r => r.Tickets)
                    .FirstOrDefaultAsync(r => !r.IsActive && string.IsNullOrEmpty(r.DrawnNumbers));
                if (activeRound != null)
                {
                    return activeRound;
                }
                return null;
            }

        }

        public async Task<Round?> GetLastRoundAsync()
        {
            // include tickets as well
            return await _context.Rounds
                .Include(r => r.Tickets)
                .OrderByDescending(r => r.OpenedAt)
                .FirstOrDefaultAsync();
        }

        public async Task AddRoundAsync(Round round)
        {
            await _context.Rounds.AddAsync(round);
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
