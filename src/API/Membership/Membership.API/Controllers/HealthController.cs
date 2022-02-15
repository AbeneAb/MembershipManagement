using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace Membership.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HealthController : ControllerBase
    {
        private readonly IHealthRepository _healthRepository;
        public HealthController(IHealthRepository healthRepository)
        {
            _healthRepository = healthRepository;
        }
        [HttpGet]
        [Route("getall")]
        [ProducesResponseType(typeof(IEnumerable<HealthInformation>),(int)HttpStatusCode.OK)]
        public async Task<ActionResult<IEnumerable<HealthInformation>>> GetAll() 
        {
            var healthInformation = await _healthRepository.GetAll();
            return Ok(healthInformation);
        }

    }
}
