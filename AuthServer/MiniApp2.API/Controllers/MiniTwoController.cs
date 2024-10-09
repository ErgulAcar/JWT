using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace MiniApp2.API.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class MiniTwoController : ControllerBase
    {

        [HttpGet]
        public IActionResult Get()
        {
            var userName = HttpContext.User.Identity.Name;

            var userIdClaim = User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier);

            return Ok($"MiniTwo İşlemleri => UserName {userName}- UserId {userIdClaim.Value}");
        }
    }
}
