namespace Membership.API.ViewModel
{
    public class CreateTransaction
    {
        public Guid MemberId { get; set; }
        public DateTime TransactionDate { get; set; }
        public string LoanNumber { get; set; }
        public Decimal Amount { get; set; }
    }
}
