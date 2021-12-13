
namespace Presentacion.Mvc.App.Models
{
    public class DatosPlaceToPay
    {
        public string Identifiquer { get; set; }
        public string IdentificationType { get; set; }
        public string IdentificationNumber { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string MobileNumber { get; set; }
        public string Email { get; set; }
        public string Application { get; set; }
        public long PaymentAmount { get; set; }
        public string Description { get; set; }
        public string UrlReturn { get; set; }
        public bool IsCreditCard { get; set; }
        public bool SavingsCard { get; set; }
        public bool RecurringPayment { get; set; }
        public bool RequiredRecurringPayment { get; set; }
    }
}