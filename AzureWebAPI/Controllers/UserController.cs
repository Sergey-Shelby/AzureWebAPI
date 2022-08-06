using AzureWebAPI.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace AzureWebAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserController : ControllerBase
    {
        [HttpGet]
        public User Get()
        {
            var user = new User()
            {
                Name = HttpContext.User.Claims.FirstOrDefault(x => x.Type == "name")?.Value,
                Email = HttpContext.User.Claims.FirstOrDefault(x => x.Type == "preferred_username")?.Value
            };

            return user;
        }
    }
}
