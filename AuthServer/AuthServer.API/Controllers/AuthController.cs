using AuthServer.Core.Configuration;
using AuthServer.Core.Dtos;
using AuthServer.Core.Models;
using AuthServer.Core.Services;
using AuthServer.Service.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SharedLibrary.Dtos;

namespace AuthServer.API.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class AuthController : CustomBaseController
    {
        private readonly IAuthenticationService _authenticationService;

        public AuthController(IAuthenticationService authenticationService)
        {
            _authenticationService = authenticationService;
        }

        [HttpPost]
        public async Task<IActionResult> CreateTokenAsync(LoginDto loginDto)
        {
            var result = await _authenticationService.CreateTokenAsync(loginDto);

            return ActionResultInstance(result);
        }

        [HttpPost]
        public IActionResult CreateTokenByClient(ClientLoginDto clientLoginDto)
        {
            var result = _authenticationService.CreateTokenByClient(clientLoginDto);

            return ActionResultInstance(result);
        }

        [HttpPost]
        public async Task<IActionResult> CreateTokenByRefreshToken(RefreshToken refreshToken)
        {
            var result = await _authenticationService.CreateTokenByRefreshToken(refreshToken.Token);

            return ActionResultInstance(result);
        }

        [HttpPost]
        public async Task<IActionResult> RevokeRefreshToken(RefreshToken refreshToken)
        {
            var result = await _authenticationService.RevokeRefreshToken(refreshToken.Token);

            return ActionResultInstance(result);
        }
    }
}
