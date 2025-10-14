using System.ComponentModel.DataAnnotations;
using System.Net.Sockets;

namespace LotoApp.Models
{
    public class Round
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();

        public bool IsActive { get; set; }

        // e.g. "3,11,22,25,38,44"
        public string? DrawnNumbers { get; set; }

        public DateTime? OpenedAt { get; set; }

        public DateTime? ClosedAt { get; set; }

        // navigation property towards Tickets
        public ICollection<Ticket> Tickets { get; set; } = new List<Ticket>();
    }
}
