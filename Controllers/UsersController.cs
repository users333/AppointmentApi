using Microsoft.AspNetCore.Mvc;

namespace AppointmentApi.Controllers
{
    [ApiController]
    [Route("users")]
    public class UsersController : ControllerBase
    {
        private static readonly List<User> Users = new()
        {
            new User { Id = 1, Name = "Admin", Role = "Admin" },
            new User { Id = 2, Name = "Maria", Role = "User" },
            new User { Id = 3, Name = "Ion", Role = "User" }
        };

        // GET /users/list
        [HttpGet("list")]
        public IActionResult GetList() => Ok(Users);

        // GET /users/details/{id}
        [HttpGet("details/{id}")]
        public IActionResult GetDetails(int id)
        {
            var user = Users.FirstOrDefault(u => u.Id == id);
            if (user == null) return NotFound();
            return Ok(user);
        }
    }
}
