using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace Membership.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MemberController : ControllerBase
    {
        private readonly IMemberRepository _memberRepository;
        private readonly ITransactionRepository _transactionRepository;
        private ILogger<MemberController> logger;

        public MemberController(IMemberRepository memberRepository, ITransactionRepository transactionRepository, ILogger<MemberController> logger)
        {
            _memberRepository = memberRepository;
            _transactionRepository = transactionRepository;
            this.logger = logger;
        }
        [HttpGet()]
        [Route("getById/{id}")]
        [ProducesResponseType(typeof(Member), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<Member>> GetByIdAsync(Guid id)
        {
            var member = await _memberRepository.GetMemberAsync(id);
            return Ok(member);
        }
        [HttpGet()]
        [Route("search/{keyword}")]
        [ProducesResponseType(typeof(IEnumerable<Member>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<IEnumerable<Member>>> MemeberByKeyword(string keyword)
        {
            keyword = keyword.ToLower();
            Expression<Func<Member, bool>> e = memeber => memeber.FirstName.ToLower().Contains(keyword) || memeber.LastName.ToLower().Contains(keyword) || memeber.Email.ToLower().Contains(keyword) || memeber.Telephone.ToLower().Contains(keyword);
            var result = await _memberRepository.FindAll(e);
            return Ok(result);
        }
        [HttpPost]
        [Route("create")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        public async Task<ActionResult<Guid>> Create([FromBody]Member member) 
        {
            var data = await _memberRepository.CreateMember(member);
            return Ok(data);
        }
        [HttpGet]
        [Route("all")]
        [ProducesResponseType(typeof(IEnumerable<Member>),(int)HttpStatusCode.OK)]
        public async Task<ActionResult<Member>> GetAll()
        {
            var data = await _memberRepository.GetAll();
            return Ok(data);
        }
    }
}
