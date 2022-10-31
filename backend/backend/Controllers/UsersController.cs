using backend.Models;
using backend.Services;
using Microsoft.AspNetCore.Mvc;
using static backend.Services.UsersService;

namespace backend.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly UsersService _usersService;

        public UsersController(UsersService usersService)
        {
            _usersService = usersService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllUsers(int page = 1, int perPage = 10, string? orderBy = "", string? orderType = "", string? filterBy = "", string? filter = "")
        {
            var users = await _usersService.GetUsers(page, perPage, orderBy, orderType, filterBy, filter);

            return Ok(new Response<IEnumerable<UserResponse>>() { Status = true, Data = users });
        }

        [HttpGet("{id}")]
        public IActionResult GetUserById(int id)
        {
            var user = _usersService.GetUserById(id);

            if(user == null)
            {
                return NotFound();
            }

            return Ok(new Response<UserResponse>() { Status = true, Data = user });
        }

        [HttpPost]
        public IActionResult CreateUser([FromBody] UserRequest value)
        {
            (UserEntryResponse response, UserResponse? user) = _usersService.CreateUser(value);

            if(response != UserEntryResponse.Created)
            {
                return response switch
                {
                    UserEntryResponse.EmailExists => Ok(new Response<string>() { Status = false, Data = "User With Same Email Exists!" }),
                    UserEntryResponse.UsernameExists => Ok(new Response<string>() { Status = false, Data = "User With Same Username Exists!" }),
                };
            }

            return Ok(new Response<UserResponse>() { Status = true, Data = user });
        }

        [HttpPut("{id}")]
        public IActionResult UpdateUser(int id, [FromBody] UserRequest value)
        {
            var response = _usersService.UpdateUser(id, value);

            if (response == UserEntryResponse.NotFound)
            {
                return NotFound();
            }

            return Ok();
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteUser(int id)
        {
            var response = _usersService.DeleteUser(id);

            if (response == UserEntryResponse.NotFound)
            {
                return NotFound();
            }

            return Ok();
        }
    }
}
