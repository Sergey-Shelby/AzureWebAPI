using AzureWebAPI.Models;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace AzureWebAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserController : ControllerBase
    {
        [HttpGet]
        public UserModel Get()
        {
            var user = new UserModel()
            {
                Name = HttpContext.User.FindFirst(ClaimTypes.Name)?.Value,
                Email = HttpContext.User.FindFirst(ClaimTypes.Email)?.Value
            };

            return user;
        }
    }
}
