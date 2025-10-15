using LotoApp.Models;

namespace LotoApp.Repositories
{
    public interface ITicketRepository
    {
        Task<Ticket?> GetByUuidAsync(string uuid);
        Task AddAsync(Ticket ticket);
        Task SaveChangesAsync();
        Task<Round?> GetActiveRoundAsync();
    }
}
