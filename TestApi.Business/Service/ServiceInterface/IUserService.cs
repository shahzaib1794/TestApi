using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Test.Business.DTO;

namespace Test.Business.Service.ServiceInterface
{
    public interface IUserService
    {
        public Task<List<User>> getUsers();
        public Task<bool> saveUser(User user);
        public Task<bool> updateUser(User user);
        public Task<bool> deleteUser(int id);

    }
}
