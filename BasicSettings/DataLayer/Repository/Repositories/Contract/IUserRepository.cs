namespace BasicSettings.DataLayer.Repository.Repositories.Contract
{
    public interface IUserRepository : IRepositoryBase<CustomeIdentityUser>
    {
        Task<CustomeIdentityUser> GetByEmail(string email);
        Task<CustomeIdentityUser> GetUserById(long userId);
        Task<CustomeIdentityUser> GetByUserName(string userName);
        Task<List<CustomeIdentityRole>> GetRolesByUserId(long userId);
        Task<List<string>> GetSystemTaskByRoleId(int roleId);
    }
}
