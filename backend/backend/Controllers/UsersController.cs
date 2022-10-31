using backend.Services;
using Data;
using Data.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using static backend.Services.UsersService;

namespace backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private UsersContext _usersContext;
        private UsersService _usersService;

        public UsersController(UsersContext usersContext, UsersService usersService)
        {
            _usersContext = usersContext;
            _usersService = usersService;
        }

        [HttpGet]
        public async Task<IEnumerable<User>> Get(int page = 1, int perPage = 10, string? orderBy = "", string? orderType = "", string? filterBy = "", string? filter = "")
        {
            return await _usersService.GetUsers(page, perPage, orderBy, orderType, filterBy, filter);
        }

        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            var user = _usersService.GetUserById(id);

            if(user == null)
            {
                return NotFound();
            }

            return Ok(user);
        }

        [HttpPost]
        public IActionResult Post([FromBody] User value)
        {
            (UserEntryResponse response, User? user) = _usersService.CreateUser(value);

            if(response == UserEntryResponse.Created)
            {
                return response switch
                {
                    UserEntryResponse.EmailExists => NotFound(),
                    UserEntryResponse.UsernameExists => NotFound(),
                };
            }

            return Ok(user);
        }

        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody] User value)
        {
            var response = _usersService.UpdateUser(id, value);

            if (response == UserEntryResponse.NotFound)
            {
                return NotFound();
            }

            return Ok();
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
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
