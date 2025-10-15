namespace LotoApp.Models
{
    public class HomeViewModel
    {
        public int TicketCount { get; set; }
        public string? DrawnNumbers { get; set; }
        public bool IsRoundActive { get; set; }
        public string? UserEmail { get; set; }
        public List<Ticket> UserTickets { get; set; } = new();

    }
}
