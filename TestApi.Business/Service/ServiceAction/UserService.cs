using Microsoft.AspNetCore.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Test.Business.DTO;
using Test.Business.HelperUtility;
using Test.Business.Service.ServiceInterface;

namespace Test.Business.Service.ServiceAction
{
    public class UserService : IUserService
    {
        private readonly IHostingEnvironment _env;
        public UserService(IHostingEnvironment env)
        {
            _env = env;
        }

        public async Task<List<User>> getUsers()
        {
            var users = (_env.ContentRootPath + "/App_Data/users.json").ConvertJsonToList<User>();
            return users;
        }
        public async Task<bool> saveUser(User user)
        {
            try
            {
                string json = JsonSerializer.Serialize(user);

                string path = (_env.ContentRootPath + "/App_Data/users.json");
                using (TextWriter tw = new StreamWriter(path))
                {
                    tw.WriteLine(json);
                };
            }
            catch (Exception ex)
            {
                throw;
            }
            return true;
        }
        public async Task<bool> updateUser(User user)
        {
            try
            {
                var users = (_env.ContentRootPath + "/App_Data/users.json").ConvertJsonToList<User>();

                users?.Where(x => x.Id == user.Id)?.ToList()?.ForEach(x =>
               {
                   x.Id = user.Id;
                   x.Name = user.Name;
                   x.Age = user.Age;
                   x.Gender = user.Gender;
                   x.Skills = user.Skills;
               });
                dynamic updatedUser = users?.Count > 1 ? users : users?.FirstOrDefault();
                string updatedUsers = JsonSerializer.Serialize(updatedUser);
                using (TextWriter tw = new StreamWriter(_env.ContentRootPath + "/App_Data/users.json"))
                {
                    tw.WriteLine(updatedUsers);
                };
                return true;
            }
            catch(Exception ex)
            {
                throw;
            }
        }
        public async Task<bool> deleteUser(int id)
        {
            var users= (_env.ContentRootPath + "/App_Data/users.json").ConvertJsonToList<User>();
            users.RemoveAll(x => x.Id == id);
            string deletedUser = JsonSerializer.Serialize(users);
            using (TextWriter tw = new StreamWriter(_env.ContentRootPath + "/App_Data/users.json"))
            {
                tw.WriteLine(deletedUser);
            };
            return true;

        }
    }
}
