
namespace Membership.API.Model
{
    public class Member : IValidatableObject
    {
        public Guid Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string? Telephone { get; set; }
        public uint? Systolic { get; set; }
        public uint? Diastolic { get; set; }
        public uint? HeartBeat { get; set; }
        public List<Transactions> Transactions { get; set; }
        public Member()
        {
            Transactions = new List<Transactions>(); 
            Id = Guid.NewGuid();
        }
        public Member(string firstName, string lastName, string email, string? telephone, uint? systolic, uint? diastolic, uint? heartBeat) : base()
        {
            FirstName = firstName;
            LastName = lastName;
            Email = email;
            Telephone = telephone;
            Systolic = systolic;
            Diastolic = diastolic;
            HeartBeat = heartBeat;
        }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            var result = new List<ValidationResult>();
            if(string.IsNullOrEmpty(FirstName)) 
            {
                result.Add(new ValidationResult("Invalid First Data", new[] { "FirstName" }));
            }
            if (string.IsNullOrEmpty(LastName)) 
            {
                result.Add(new ValidationResult("Invalid Lastname Data", new[] { "LastName" }));
            }
            if (string.IsNullOrEmpty(Email)) 
            {
                result.Add(new ValidationResult("Invalid Data", new[] { "Email" }));
            }
            if(Systolic < 0)
            {
                result.Add(new ValidationResult("Invalid Data", new[] { "Systolic" }));
            }
            if (Diastolic < 0)
            {
                result.Add(new ValidationResult("Invalid Data", new[] { "Diastolic" }));
            }
            if (HeartBeat < 0)
            {
                result.Add(new ValidationResult("Invalid Data", new[] { "HeartBeat" }));
            }
            return result;
        }
    }
}
