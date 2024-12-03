namespace PaymentsAPI.Models
{
    public class Payment
    {
        public Guid TransactionID { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string ModeOfPayment { get; set; }
    }
}