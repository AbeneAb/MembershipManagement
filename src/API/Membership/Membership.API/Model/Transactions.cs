namespace Membership.API.Model
{
    public class Transactions : IValidatableObject
    {
        public Transactions()
        {

        }
        public Transactions(Guid memberId, DateTime transactionDate, string loanNumber, decimal amount)
        {
            MemberId = memberId;
            TransactionDate = transactionDate;
            LoanNumber = loanNumber;
            Amount = amount;
        }

        public Guid Id { get; set; }
        public Guid MemberId { get; set; }
        public DateTime TransactionDate { get; set; }   
        public string LoanNumber { get; set; }
        public Decimal Amount { get; set; }
        public virtual Member Member { get; set; }
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            var result = new List<ValidationResult>();
            if(Amount <= 0)
            {
                result.Add(new ValidationResult("Invalid Amount data", new[] { "Amount" }));
            }
            return result;
        }
    }
}
