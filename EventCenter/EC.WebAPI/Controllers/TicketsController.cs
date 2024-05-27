using EC.ApplicationServices.Interfaces;
using EC.Infrastructure.Messaging;
using EC.Infrastructure.Messaging.Requests;
using EC.Infrastructure.Messaging.Requests.EventsRequests;
using EC.Infrastructure.Messaging.Requests.TicketsRequests;
using EC.Infrastructure.Messaging.Requests.UserssRequests;
using EC.Infrastructure.Messaging.Responses.EventsResponses;
using EC.Infrastructure.Messaging.Responses.TicketsResponses;
using EC.Infrastructure.Messaging.Responses.UsersResponses;
using Microsoft.AspNetCore.Mvc;

namespace EC.WebAPI.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    [Produces("application/json")]
    public class TicketsController : Controller
    {

        private readonly ITicketsManagementService _ticketService;

        public TicketsController(ITicketsManagementService ticketService)
        {
            _ticketService = ticketService;
        }

        [HttpGet]
        [ProducesResponseType(typeof(GetTicketResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ServiceResponseError), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Get([FromQuery] bool isActive = true) => Ok(await _ticketService.GetTicket(new(isActive)));

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(GetTicketResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ServiceResponseError), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetTicketById([FromRoute] int id) => Ok(await _ticketService.GetTicketById(id));

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(GetTicketResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ServiceResponseError), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> SearchTicketsByEventId(int id) => Ok(await _ticketService.SearchTicketsByEventId(id));

        //[HttpPost]
        //[ProducesResponseType(typeof(CreateUserResponse), StatusCodes.Status200OK)]
        //[ProducesResponseType(StatusCodes.Status400BadRequest)]
        //[ProducesResponseType(typeof(ServiceResponseError), StatusCodes.Status500InternalServerError)]
        //public async Task<IActionResult> Create([FromBody] UserModel user) => Ok(await _userService.CreateUser(new(user)));

        [HttpPost]
        [ProducesResponseType(typeof(CreateTicketResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ServiceResponseError), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> CreateTicket([FromBody] CreateTicketRequest request) => Ok(await _ticketService.CreateTicket(new(request.Ticket)));
        

        [HttpDelete("{id}")]
        [ProducesResponseType(typeof(DeleteTicketResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ServiceResponseError), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> DeleteTicket([FromRoute] int id) => Ok(await _ticketService.DeleteTicket(new(id)));

        [HttpPut("{id}")]
        [ProducesResponseType(typeof(UpdateTicketResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ServiceResponseError), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> UpdateTicket([FromRoute] int id, [FromBody] UpdateTicketRequest updateRequest) => Ok(await _ticketService.UpdateTicket(new(id, updateRequest.Ticket)));


    }
}
