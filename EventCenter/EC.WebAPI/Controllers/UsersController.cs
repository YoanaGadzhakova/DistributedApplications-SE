using EC.ApplicationServices.Interfaces;
using EC.Infrastructure.Messaging;
using EC.Infrastructure.Messaging.Requests;
using EC.Infrastructure.Messaging.Requests.UserssRequests;
using EC.Infrastructure.Messaging.Responses;
using EC.Infrastructure.Messaging.Responses.UsersResponses;
using Microsoft.AspNetCore.Mvc;

namespace EC.WebAPI.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    [Produces("application/json")]
    public class UsersController : Controller
    {

        private readonly IUsersManagementService _userService;

        public UsersController(IUsersManagementService userService)
        {
            _userService = userService;
        }

        [HttpGet]
        [ProducesResponseType(typeof(GetUserResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ServiceResponseError), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Get([FromQuery] bool isActive = true) => Ok(await _userService.GetUser(new(isActive)));

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(GetUserResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ServiceResponseError), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetUserById([FromRoute] int id) => Ok(await _userService.GetUserById(id));

        [HttpGet("{firstName}")]
        [ProducesResponseType(typeof(GetUserResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ServiceResponseError), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> SearchUsersByFirstName(string firstName) => Ok(await _userService.SearchUsersByFirstName(firstName));

        //[HttpPost]
        //[ProducesResponseType(typeof(CreateUserResponse), StatusCodes.Status200OK)]
        //[ProducesResponseType(StatusCodes.Status400BadRequest)]
        //[ProducesResponseType(typeof(ServiceResponseError), StatusCodes.Status500InternalServerError)]
        //public async Task<IActionResult> Create([FromBody] UserModel user) => Ok(await _userService.CreateUser(new(user)));

        [HttpPost]
        [ProducesResponseType(typeof(CreateUserResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ServiceResponseError), StatusCodes.Status500InternalServerError)]
        //public async Task<IActionResult> Create([FromBody] CreateUserRequest request) => Ok(await _userService.CreateUser(new(request.User)));
        public async Task<IActionResult> CreateUser([FromBody] UserModel request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var result = await _userService.CreateUser(new(request));
                return Ok(result);
            }
            catch (Exception ex)
            {
                // Log the exception and return an error response
                return StatusCode(500, new { message = "An error occurred while creating the user." });
            }
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(typeof(DeleteUserResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ServiceResponseError), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> DeleteUser([FromRoute] int id) => Ok(await _userService.DeleteUser(new(id)));

        [HttpPut("{id}")]
        [ProducesResponseType(typeof(UpdateUserResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ServiceResponseError), StatusCodes.Status500InternalServerError)]
        public IActionResult UpdateUser([FromRoute] int id, [FromBody] UpdateUserRequest updateRequest) => Ok( _userService.UpdateUser(new(id, updateRequest.User)));


    }
}
