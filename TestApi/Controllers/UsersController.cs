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
        public async Task<List<User>> GetUsers()
        {
            return await _userService.getUsers();
        }
        [HttpPost("save")]
        public async Task<bool> SaveUser(List<User> user)
        {
            return await _userService.saveUser(user);
        }
        [HttpPost("update")]
        public async Task<bool> UpdateUser(User user)
        {
            return await _userService.updateUser(user);
        }
        [HttpDelete("delete/{id}")]
        public async Task<bool> DeleteUser(int id)
        {
            return await _userService.deleteUser(id);
        }
    }
}
