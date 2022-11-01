using Core.BindingModels;
using Microsoft.AspNetCore.Mvc;

namespace backend.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class PermisionsController : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> GetAllPermisions()
        {
            return Ok();
        }

       /* [HttpPost]
        public IActionResult CreatePermission([FromBody] PermissionBindingModel value)
        {
            if (!ModelState.IsValid)
            {
                return UnprocessableEntity(ModelState);
            }

            (UserEntryResponse response, UserDto? user) = _usersService.CreateUser(_mapper.Map<UserWithPasswordDto>(value));


            return Ok(new Response<UserDto>() { Status = true, Data = user });
        }*/
    }
}
