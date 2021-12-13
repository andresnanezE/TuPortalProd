namespace EmermedicaReservaNit.Models
{
    public class DataClient
    {
        public int ValidationDigit { get; set; }
        public string CompanyName { get; set; }
        public string ContactName { get; set; }
        public string ContactCharge { get; set; }
        public string Email { get; set; }
        public string NumberPhone { get; set; }
        public int Extension { get; set; }
        public string MobilePhone { get; set; }
        public int ProductId { get; set; }
        public int UserId { get; set; }
        public decimal Nit { get; set; }
        public string ChannelName { get; set; }
        public string CityName { get; set; }
        public string Annotation { get; set; }
    }
}