namespace autorent.Models
{
    public class Rental
    {
        public Car Car { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
        public int Price { get; set; }
        public string RentalTimestamp { get; set; }
    }
}
