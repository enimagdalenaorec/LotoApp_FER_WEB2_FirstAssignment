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
            return await _context.Rounds.FirstOrDefaultAsync(r => r.IsActive);
        }

        public async Task<Round?> GetLastRoundAsync()
        {
            return await _context.Rounds
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
