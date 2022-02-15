namespace Membership.API.Interfaces
{
    public interface ITransactionRepository
    {
        Task<Guid> CreateTransaction(Transactions transacation); 
        Task<IEnumerable<Transactions>> GetTransactionsByMembers(Guid memberId);
        Task<IEnumerable<Transactions>> GetAllTransactions();
    }
}
