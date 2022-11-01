using AutoMapper;
using backend.Models;
using Core.BindingModels;
using Core.Dto;
using Core.Models;
using Core.Services;
using Microsoft.AspNetCore.Mvc;
using static Core.Services.UsersService;

namespace backend.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly UsersService _usersService;
        private readonly IMapper _mapper;

        public UsersController(UsersService usersService, IMapper mapper)
        {
            _usersService = usersService;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllUsers(int page = 1, int perPage = 10, string? orderBy = "", string? orderType = "", string? filterBy = "", string? filter = "")
        {
            var users = await _usersService.GetUsers(page, perPage, orderBy, orderType, filterBy, filter);

            return Ok(new Response<PaginatedResponse<UserDto>>() { Status = true, Data = users });
        }

        [HttpGet("{id}")]
        public IActionResult GetUserById(int id)
        {
            var user = _usersService.GetUserById(id);

            if (user == null)
            {
                return NotFound();
            }

            return Ok(new Response<UserDto>() { Status = true, Data = user });
        }

        [HttpPost]
        public IActionResult CreateUser([FromBody] CreateUserBindingModel value)
        {
            if (!ModelState.IsValid)
            {
                return UnprocessableEntity(ModelState);
            }

            (UserEntryResponse response, UserDto? user) = _usersService.CreateUser(_mapper.Map<UserWithPasswordDto>(value));

            if (response != UserEntryResponse.Created)
            {
                return response switch
                {
                    UserEntryResponse.EmailExists => Ok(new Response<string>() { Status = false, Data = "User With Same Email Exists!" }),
                    UserEntryResponse.UsernameExists => Ok(new Response<string>() { Status = false, Data = "User With Same Username Exists!" }),
                };
            }

            return Ok(new Response<UserDto>() { Status = true, Data = user });
        }

        [HttpPut("{id}")]
        public IActionResult UpdateUser(int id, [FromBody] UpdateUserBindingModel value)
        {
            if (!ModelState.IsValid)
            {
                return UnprocessableEntity(ModelState);
            }

            var response = _usersService.UpdateUser(id, _mapper.Map<UserDto>(value));

            if (response == UserEntryResponse.NotFound)
            {
                return NotFound();
            }

            if (response == UserEntryResponse.EmailExists)
            {
                return Ok(new Response<string>() { Status = false, Data = "User With Same Email Exists!" });
            }

            return Ok(new Response<string>() { Status = true, Data = "Updated" });
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
