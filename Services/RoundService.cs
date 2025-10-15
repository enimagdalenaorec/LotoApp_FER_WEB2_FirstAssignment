using LotoApp.Models;
using LotoApp.Repositories;

namespace LotoApp.Services
{
    public class RoundService
    {
        private readonly IRoundRepository _roundRepository;

        public RoundService(IRoundRepository roundRepository)
        {
            _roundRepository = roundRepository;
        }

        public async Task<bool> StartNewRoundAsync()
        {
            var active = await _roundRepository.GetActiveRoundAsync();
            if (active != null) return false;

            var newRound = new Round
            {
                IsActive = true,
                OpenedAt = DateTime.UtcNow
            };

            await _roundRepository.AddRoundAsync(newRound);
            await _roundRepository.SaveChangesAsync();
            return true;
        }

        public async Task<bool> CloseCurrentRoundAsync()
        {
            var active = await _roundRepository.GetActiveRoundAsync();
            if (active == null) return false;

            active.IsActive = false;
            active.ClosedAt = DateTime.UtcNow;

            await _roundRepository.SaveChangesAsync();
            return true;
        }

        public async Task<bool> StoreResultsAsync(List<int> numbers)
        {
            var lastRound = await _roundRepository.GetLastRoundAsync();
            if (lastRound == null || lastRound.IsActive || lastRound.DrawnNumbers != null)
                return false;

            lastRound.DrawnNumbers = string.Join(",", numbers);
            await _roundRepository.SaveChangesAsync();
            return true;
        }
    }
}
