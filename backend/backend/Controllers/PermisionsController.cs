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
    }
}
