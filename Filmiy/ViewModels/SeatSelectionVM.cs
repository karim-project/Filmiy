namespace Filmiy.ViewModels
{
    public class SeatSelectionVM
    {
        public int MovieId { get; set; }
        public string MovieTitle { get; set; }
        public decimal PriceStandard { get; set; }
        public decimal PricePremium { get; set; }

      
        public List<string> ReservedSeats { get; set; } = new();
    }
}
