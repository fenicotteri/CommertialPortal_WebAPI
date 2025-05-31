using CommertialPortal_WebAPI.Features.Posts.GetBranches;
using CommertialPortal_WebAPI.Features.Posts.GetBranchesByBusinessId;
using CommertialPortal_WebAPI.Features.Users.GetBusinessById;
using CommertialPortal_WebAPI.Features.Users.GetBusinesses;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CommertialPortal_WebAPI.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BusinessController : ControllerBase
    {
        private readonly IMediator _mediator;

        public BusinessController(IMediator mediator) => _mediator = mediator;

        [HttpGet("{businessId}/branches")]
        public async Task<ActionResult<List<BranchDto>>> GetBranchesByBusinessId(int businessId)
        {
            var branches = await _mediator.Send(new GetBranchesByBusinessIdQuery(businessId));
            return Ok(branches);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<BusinessProfileDto>> GetBusinessById(int id)
        {
            var business = await _mediator.Send(new GetBusinessByIdQuery(id));

            if (business == null)
            {
                return NotFound();
            }

            return Ok(business);
        }

        [HttpGet]
        public async Task<ActionResult<List<BusinessProfileDto>>> GetAllBusinesses()
        {
            var businesses = await _mediator.Send(new GetBusinessesQuery());
            return Ok(businesses);
        }
    }
}
