namespace Membership.API.Infrastrucuture
{
    public class MembershipContextSeed
    {
        public static async Task SeedAsync(MembershipContext membershipContext,ILogger<MembershipContextSeed> logger) 
        {
            if (!membershipContext.Members.Any()) 
            {
                membershipContext.Members.AddRange(SeedMembers());
            }
            if (!membershipContext.HealthInformation.Any()) 
            {
                membershipContext.HealthInformation.AddRange(SeedHealthInformation());
            }
           await membershipContext.SaveChangesAsync();
        }
        public static IEnumerable<Member> SeedMembers()
        {
            List<Member> members = new List<Member>();
            members.Add(new Member()
            {
                FirstName = "John",
                Diastolic = 80,
                Systolic = 120,
                Email = "johndoe@email.com",
                HeartBeat = 72,
                LastName = "Doe",
                Id = Guid.NewGuid(),
                Telephone = "+251 911317649",
                Transactions = new List<Transactions> { new Transactions() { Amount = 100, TransactionDate = DateTime.Now, Id = Guid.NewGuid(),LoanNumber= "LN-0090" } },


            });
            members.Add(new Member()
            {
                FirstName = "Jane",
                Diastolic = 80,
                Systolic = 120,
                Email = "jahndoe@email.com",
                HeartBeat = 72,
                LastName = "Doe",
                Id = Guid.NewGuid(),
                Telephone = "+251 911317645",
            });
            members.Add(new Member()
            {
                FirstName = "Jill",
                Diastolic = 80,
                Systolic = 120,
                Email = "jilldoe@email.com",
                HeartBeat = 72,
                LastName = "Doe",
                Id = Guid.NewGuid(),
                Telephone = "+251 911317685",
            });
            return members;
        }
        public static IEnumerable<HealthInformation> SeedHealthInformation() 
        {
            var members = new List<HealthInformation>();
            members.Add(new HealthInformation() { DeviceId ="Device 1", Diastolic = 80, Systolic = 120, HeartRate = 74, Time = new DateTime(2021, 9, 10) });
            members.Add(new HealthInformation() { DeviceId = "Device 2", Diastolic = 90, Systolic = 130, HeartRate = 68, Time = new DateTime(2022, 1, 10) });
            members.Add(new HealthInformation() { DeviceId = "Device ", Diastolic = 80, Systolic = 110, HeartRate = 60, Time = new DateTime(2021, 12, 10) });
            members.Add(new HealthInformation() { DeviceId = "Device 4", Diastolic = 80, Systolic = 130, HeartRate = 37, Time = new DateTime(2022, 2, 10) });

            members.Add(new HealthInformation() { DeviceId = "Device 5", Diastolic = 90, Systolic = 140, HeartRate = 49, Time = new DateTime(2021, 8, 10) });

            members.Add(new HealthInformation() { DeviceId = "Device 1", Diastolic = 70, Systolic = 110, HeartRate = 110, Time = new DateTime(2022, 2, 10) });

            members.Add(new HealthInformation() { DeviceId = "Device 6", Diastolic = 100, Systolic = 190, HeartRate = 70, Time = new DateTime(2022, 1, 12) });

            members.Add(new HealthInformation() { DeviceId = "Device 7", Diastolic = 96, Systolic = 120, HeartRate = 90, Time = new DateTime(2022,1 , 28) });

            members.Add(new HealthInformation() { DeviceId = "Device 8", Diastolic = 88, Systolic = 142, HeartRate = 53, Time = new DateTime(2021, 12, 10) });

            members.Add(new HealthInformation() { DeviceId = "Device 9", Diastolic = 100, Systolic = 140, HeartRate = 45, Time = new DateTime(2021,11, 19) });
            return members;

        }
    }
}
