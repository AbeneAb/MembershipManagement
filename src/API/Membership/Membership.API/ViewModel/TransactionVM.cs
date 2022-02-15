namespace Membership.API.ViewModel
{
    public class TransactionVM
    {
        public Guid Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public decimal Amount { get; set; }
        public string LoanNumber { get; set; }
        public string Email { get; set; }
        public DateTime TransactionDate { get; set; }
    }
}
