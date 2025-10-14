using System.ComponentModel.DataAnnotations;

namespace LotoApp.Models
{
    public class Ticket
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();

        [Required, StringLength(20)]
        public string DocumentNumber { get; set; } // personal id document number or passport number

        [Required]
        public string SelectedNumbers { get; set; } // e.g. "1,5,7,11,23,44"

        [Required]
        public string UUID { get; set; } = Guid.NewGuid().ToString(); // for QR code

        // Foreign key to Round
        public Guid RoundId { get; set; }
        public Round Round { get; set; }

        // Foreign key to User 
        public Guid? UserId { get; set; }
        public User? User { get; set; }
    }
}
