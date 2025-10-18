using LotoApp.Models;
using LotoApp.Repositories;
using QRCoder;

namespace LotoApp.Services
{
    public class TicketService
    {
        private readonly ITicketRepository _ticketRepository;
        private readonly IRoundRepository _roundRepository;

        public TicketService(ITicketRepository ticketRepository, IRoundRepository roundRepository)
        {
            _ticketRepository = ticketRepository;
            _roundRepository = roundRepository;
        }

        public async Task<(byte[]? qrCode, string? error)> CreateTicketAsync(string documentNumber, string numbersCsv, Guid userId)
        {
            // validate document number
            if (string.IsNullOrWhiteSpace(documentNumber) || documentNumber.Length > 20)
                return (null, "Incorrect personal ID or passport number.");

            // validate numbers
            var numbers = numbersCsv.Split(',').Select(n => n.Trim()).ToList();

            if (numbers.Count < 6 || numbers.Count > 10)
                return (null, "You must choose between 6 and 10 numbers.");

            if (!numbers.All(n => int.TryParse(n, out int num) && num >= 1 && num <= 45))
                return (null, "All numbers must be between 1 and 45.");

            if (numbers.Distinct().Count() != numbers.Count)
                return (null, "Numbers must not repeat.");

            // check for active round
            var activeRound = await _roundRepository.GetActiveRoundAsync();
            if (activeRound == null)
                return (null, "There is no active round for ticket submission.");

            var ticket = new Ticket
            {
                DocumentNumber = documentNumber,
                SelectedNumbers = string.Join(",", numbers),
                RoundId = activeRound.Id,
                UserId = userId,
                UUID = Guid.NewGuid().ToString()
            };

            await _ticketRepository.AddAsync(ticket);
            await _ticketRepository.SaveChangesAsync();

            // generate QR code with public URL
            var qrUrl = $"https://lotoapp-fer-web2-firstassignment.onrender.com/Ticket/{ticket.UUID}"; 
            var qrCodeBytes = GenerateQrCode(qrUrl);

            return (qrCodeBytes, null);
        }

        public async Task<Ticket?> GetTicketByUuidAsync(string uuid)
        {
            return await _ticketRepository.GetByUuidAsync(uuid);
        }

        public byte[] GenerateQrCode(string content)
        {
            using var qrGenerator = new QRCodeGenerator();
            using var qrData = qrGenerator.CreateQrCode(content, QRCodeGenerator.ECCLevel.Q);
            using var qrCode = new PngByteQRCode(qrData);
            return qrCode.GetGraphic(20);
        }
    }
}
