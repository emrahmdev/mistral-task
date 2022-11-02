using AutoMapper;
using backend.Models;
using Core.Dto;
using Core.Services;
using Microsoft.AspNetCore.Mvc;

namespace backend.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class PermissionsController : ControllerBase
    {

        private readonly PermissionsService _permisionsService;
        private readonly IMapper _mapper;

        public PermissionsController(PermissionsService permisionsService, IMapper mapper)
        {
            _permisionsService = permisionsService;
            _mapper = mapper;
        }


        [HttpGet]
        public async Task<IActionResult> GetAllPermissions()
        {
            var permission = await _permisionsService.GetAllPermissions();

            return Ok(new Response<IEnumerable<PermissionDto>>() { Status = true, Data = permission });
        }

        [HttpGet("{userId}")]
        public async Task<IActionResult> GetUserPermissions(long userId)
        {
            var userPermissions = await _permisionsService.GetUserPermissions(userId);

            if (userPermissions == null)
            {
                return NotFound();
            }

            return Ok(new Response<IEnumerable<PermissionDto>>() { Status = true, Data = userPermissions });
        }

        [HttpGet("{userId}/{permissionId}")]
        public async Task<IActionResult> AssignUserPermission(long userId, long permissionId)
        {
            var status = await _permisionsService.AssignUserPermission(userId, permissionId);

            if (status == false)
            {
                return NotFound();
            }

            return Ok();
        }

        [HttpGet("{userId}/{permissionId}")]
        public async Task<IActionResult> RevokeUserPermission(long userId, long permissionId)
        {
            var status = await _permisionsService.RevokeUserPermission(userId, permissionId);

            if (status == false)
            {
                return NotFound();
            }

            return Ok();
        }
    }
}
