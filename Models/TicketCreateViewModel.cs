using System.ComponentModel.DataAnnotations;

namespace LotoApp.Models
{
    public class TicketCreateViewModel
    {
        [Required(ErrorMessage = "Document number is required.")]
        [StringLength(20, ErrorMessage = "Document number cannot exceed 20 characters.")]
        public string DocumentNumber { get; set; } = null!;

        [Required(ErrorMessage = "You must provide selected numbers.")]
        public string SelectedNumbers { get; set; } = null!;
    }
}
