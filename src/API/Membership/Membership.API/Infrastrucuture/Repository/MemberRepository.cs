

namespace Membership.API.Infrastrucuture.Repository
{
    public class MemberRepository : IMemberRepository
    {
        private readonly ILogger<TransactionRepository> _logger;
        private readonly MembershipContext _context;
        public MemberRepository(ILogger<TransactionRepository> logger, MembershipContext context)
        {
            _logger = logger;
            _context = context;
        }
        public async Task<Guid> CreateMember(Member member)
        {
            Member newMember = new Member(member.FirstName,member.LastName,member.Email,member.Telephone,member.Systolic,member.Diastolic,member.HeartBeat);
            _context.Members.Add(newMember);
            var result = await _context.SaveChangesAsync();
            return newMember.Id;
        }
        public async Task<IEnumerable<Member>> FindAll(Expression<Func<Member, bool>> predicate = null)
        {
            var memebers = _context.Members.Where(predicate).AsNoTracking().ToListAsync();
            return await memebers;
        }

        public async Task<IEnumerable<Member>> GetAll()
        {
            var memebers = _context.Members.AsNoTracking().ToListAsync();
            return await memebers;
        }

        public async Task<Member> GetMemberAsync(Guid id)
        {
           var memeber = await _context.Members.Include(m=>m.Transactions).FirstOrDefaultAsync(m=>m.Id == id);
            return memeber;
        }
    }
}
