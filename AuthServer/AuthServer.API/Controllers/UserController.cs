using AuthServer.Core.Dtos;
using AuthServer.Core.Models;
using AuthServer.Core.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SharedLibrary.Dtos;

namespace AuthServer.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : CustomBaseController
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost]
        public async Task<IActionResult> CreateUserAsync(CreateUserDto createUserDto)
        {
            return ActionResultInstance(await _userService.CreateUserAsync(createUserDto));
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> GetUserByNameAsync()
        {
            return ActionResultInstance(await _userService.GetUserByNameAsync(HttpContext.User.Identity.Name));
        }
    }
}
