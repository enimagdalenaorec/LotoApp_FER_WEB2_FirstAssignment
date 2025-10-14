using System.ComponentModel.DataAnnotations;

namespace LotoApp.Models
{
    public class User
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();

        public string Auth0Sub { get; set; } // Auth0 identificator

        public string? Email { get; set; }   // optional

        public ICollection<Ticket> Tickets { get; set; } = new List<Ticket>();
    }
}
