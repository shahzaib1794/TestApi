using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Test.Business.DTO;
using Test.Business.Service.ServiceInterface;

namespace TestApi.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;
        public UsersController(IUserService userService)
        {
            _userService = userService;
        }
        [HttpGet("get")]
        public async Task<IActionResult> GetUsers()
        {
            var users = await _userService.getUsers();
            return Ok(users);
        }
        [HttpPost("save")]
        public async Task<IActionResult> SaveUser(List<User> user)
        {
            var savedStatus = await _userService.saveUser(user);
            return Ok(savedStatus);
        }
        [HttpPost("update")]
        public async Task<IActionResult> UpdateUser(User user)
        {
            var updatedUserStatus = await _userService.updateUser(user);
            return Ok(updatedUserStatus);
        }
        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            var deletedUserStatus = await _userService.deleteUser(id);
            return Ok(deletedUserStatus);
        }
    }
}
