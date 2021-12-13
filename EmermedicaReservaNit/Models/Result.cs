using System.Collections.Generic;

namespace EmermedicaReservaNit.Models
{
    public class Result
    {
        public bool IsSuccess { get; set; }
        public string Message { get; set; }
        public List<string> ValidationList { get; set; }
        public InformationBusinness Information { get; set; }
    }
}