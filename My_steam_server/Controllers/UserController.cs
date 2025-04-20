using Microsoft.AspNetCore.Mvc;
using My_steam_server.Interfaces;
using My_steam_server.Models;

namespace My_steam_server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IUserRepository _repository;

        public UserController(IUserRepository repository)
        {
            _repository = repository;
        }

        [HttpGet]
        public IActionResult GetAllUsers()
        {
            var users = _repository.GetAllAsync();
            return Ok(users);
        }

        [HttpPost]
        public IActionResult AddUser(User user)
        {
            _repository.AddUserAsync(user);
            _repository.SaveChangesAsync();
            return CreatedAtAction(nameof(GetAllUsers), new { id = user.Id }, user);
        }
    }
}
