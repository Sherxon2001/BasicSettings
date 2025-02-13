using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BasicSettings.DataLayer.Repository.Repositories
{
    public interface ICustomeIdentityUserRepository : IRepositoryBase<CustomeIdentityUser>
    {
        Task<CustomeIdentityUser> GetByEmail(string email);
        Task<CustomeIdentityUser> GetUserById(int userId);
        Task<CustomeIdentityUser> GetByUserName(string userName);
        Task<List<CustomeIdentityRole>> GetRolesByUserId(int userId);
        Task<List<string>> GetSystemTaskByRoleId(int roleId);
    }
}
