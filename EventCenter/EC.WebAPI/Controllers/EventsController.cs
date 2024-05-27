using EC.ApplicationServices.Interfaces;
using EC.Infrastructure.Messaging;
using EC.Infrastructure.Messaging.Requests;
using EC.Infrastructure.Messaging.Requests.EventsRequests;
using EC.Infrastructure.Messaging.Requests.UserssRequests;
using EC.Infrastructure.Messaging.Responses.EventsResponses;
using EC.Infrastructure.Messaging.Responses.UsersResponses;
using Microsoft.AspNetCore.Mvc;

namespace EC.WebAPI.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    [Produces("application/json")]
    public class EventsController : Controller
    {

        private readonly IEventsManagementService _eventService;

        public EventsController(IEventsManagementService eventService)
        {
            _eventService = eventService;
        }

        [HttpGet]
        [ProducesResponseType(typeof(GetEventResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ServiceResponseError), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Get([FromQuery] bool isActive = true) => Ok(await _eventService.GetEvent(new(isActive)));

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(GetEventResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ServiceResponseError), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetEventById([FromRoute] int id) => Ok(await _eventService.GetEventById(id));

        [HttpGet("{title}")]
        [ProducesResponseType(typeof(GetEventResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ServiceResponseError), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> SearchEventsByTitle(string title) => Ok(await _eventService.SearchEventsByTitle(title));

        //[HttpPost]
        //[ProducesResponseType(typeof(CreateUserResponse), StatusCodes.Status200OK)]
        //[ProducesResponseType(StatusCodes.Status400BadRequest)]
        //[ProducesResponseType(typeof(ServiceResponseError), StatusCodes.Status500InternalServerError)]
        //public async Task<IActionResult> Create([FromBody] UserModel user) => Ok(await _userService.CreateUser(new(user)));

        [HttpPost]
        [ProducesResponseType(typeof(CreateEventResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ServiceResponseError), StatusCodes.Status500InternalServerError)]
        //public async Task<IActionResult> Create([FromBody] CreateUserRequest request) => Ok(await _userService.CreateUser(new(request.User)));
        public async Task<IActionResult> CreateEvent([FromBody] EventModel tevent) => Ok(await _eventService.CreateEvent(new(tevent)));

        [HttpDelete("{id}")]
        [ProducesResponseType(typeof(DeleteEventResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ServiceResponseError), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> DeleteEvent([FromRoute] int id) => Ok(await _eventService.DeleteEvent(new(id)));

        [HttpPut("{id}")]
        [ProducesResponseType(typeof(UpdateEventResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ServiceResponseError), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> UpdateEvent([FromRoute] int id, [FromBody] UpdateEventRequest updateRequest) => Ok(await _eventService.UpdateEvent(new(id, updateRequest.Event)));


    }
}
