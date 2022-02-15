using Membership.API.ViewModel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace Membership.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TransactionController : ControllerBase
    {
        private readonly ITransactionRepository _transactionRepository;

        public TransactionController(ITransactionRepository transactionRepository)
        {
            _transactionRepository = transactionRepository;
        }
        [Route("create")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [HttpPost]
        public async Task<ActionResult<Guid>> CreateTransaction(CreateTransaction transactions)
        {
            Transactions transaction = new Transactions(transactions.MemberId,transactions.TransactionDate,transactions.LoanNumber,transactions.Amount);
            var result = await _transactionRepository.CreateTransaction(transaction);
            return Ok(result);
        }
        [Route("getall")]
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<TransactionVM>),(int)HttpStatusCode.OK)]
        public async Task<ActionResult<IEnumerable<TransactionVM>>> GetAllTransactions() 
        {
            var data = await _transactionRepository.GetAllTransactions();
            var result = data.Select(d => new TransactionVM
            {
                Amount = d.Amount,
                Email = d.Member.Email,
                FirstName = d.Member.FirstName,
                LastName = d.Member.LastName,
                LoanNumber = d.LoanNumber,
                Id = d.Id,
                TransactionDate = d.TransactionDate
            });
            return Ok(result);
        }
        [Route("getformember")]
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<TransactionVM>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<IEnumerable<TransactionVM>>> GetMemberTransactions(Guid memberId)
        {
            var data = await _transactionRepository.GetTransactionsByMembers(memberId);
            var result = data.Select(d => new TransactionVM
            {
                Amount = d.Amount,
                Email = d.Member.Email,
                FirstName = d.Member.FirstName,
                LastName = d.Member.LastName,
                LoanNumber = d.LoanNumber,
                Id = d.Id,
                TransactionDate = d.TransactionDate
            });
            return Ok(result);
        }

    }
}
