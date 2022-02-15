using Membership.API.Interfaces;

namespace Membership.API.Infrastrucuture.Repository
{
    public class TransactionRepository : ITransactionRepository
    {
        private readonly ILogger<TransactionRepository> _logger;
        private readonly MembershipContext _context;
    
        public TransactionRepository(ILogger<TransactionRepository> logger, MembershipContext context)
        {
            _logger = logger;   
            _context = context;
        }

        public async Task<Guid> CreateTransaction(Transactions transacation)
        {
             await _context.Transactions.AddAsync(transacation);
            var result = await _context.SaveChangesAsync();
            return transacation.Id;
        }

        public async Task<IEnumerable<Transactions>> GetTransactionsByMembers(Guid memberId)
        {
            var result = _context.Transactions.Include(t=>t.Member).Where(t=>t.MemberId==memberId).AsNoTracking().ToListAsync();
            return await result;
        }
        public async Task<IEnumerable<Transactions>> GetAllTransactions() 
        {
            var result = _context.Transactions.Include(t => t.Member).AsNoTracking().ToListAsync();
            return await result;
        }
    }
}
