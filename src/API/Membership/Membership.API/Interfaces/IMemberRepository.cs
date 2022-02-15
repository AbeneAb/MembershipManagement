namespace Membership.API.Interfaces
{
    public interface IMemberRepository
    {
        Task<Guid> CreateMember(Member member);
        Task<IEnumerable<Member>> GetAll();
        Task<IEnumerable<Member>> FindAll(Expression<Func<Member, bool>> predicate = null);
        Task<Member> GetMemberAsync(Guid id);
    }
}
