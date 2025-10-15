using LotoApp.Models;

namespace LotoApp.Repositories
{
    public interface IRoundRepository
    {
        Task<Round?> GetActiveRoundAsync();
        Task<Round?> GetLastRoundAsync();
        Task AddRoundAsync(Round round);
        Task SaveChangesAsync();
    }
}
