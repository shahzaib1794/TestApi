using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Test.Business.Service.ServiceAction;
using Test.Business.Service.ServiceInterface;
using TestApi.Controllers;
using Xunit;

namespace UnitTestProject
{
    public class UserControllerTest
    {
        private readonly UsersController _controller;
        private readonly IUserService _userService;

        public UserControllerTest()
        {
            _userService = new UserService();
            _controller = new UsersController(_userService);
        }
        [Fact]
        public async Task Get_WhenCalled_ReturnsOkResult()
        {
            // Act
            var okResult = await _controller.GetUsers();
            // Assert
            Assert.IsType<OkObjectResult>(okResult as OkObjectResult);
        }
        [Fact]
        public async Task Save_WhenCalled_ReturnsOkResult()
        {
            // Act
            var user = new List<Test.Business.DTO.User>();
            user.Add(new Test.Business.DTO.User { Id = 5, Name = "TestCaseUser", Age = 22, Gender = "Male", Skills = new List<string> { "testing" } });
            var okResult = await _controller.SaveUser(user);
            // Assert
            Assert.IsType<OkObjectResult>(okResult as OkObjectResult);
        }
        [Fact]
        public async Task Update_WhenCalled_ReturnsOkResult()
        {
            // Act
            var user = new Test.Business.DTO.User() { Id = 5, Name = "UpdateTestCaseUser", Age = 25, Gender = "Male", Skills = new List<string> { "update testing" } };
            var okResult = await _controller.UpdateUser(user);
            // Assert
            Assert.IsType<OkObjectResult>(okResult as OkObjectResult);
        }
        [Fact]
        public async Task Delete_WhenCalled_ReturnsOkResult()
        {
            // Act
            var okResult = await _controller.DeleteUser(5);
            // Assert
            Assert.IsType<OkObjectResult>(okResult as OkObjectResult);
        }
    }
}
