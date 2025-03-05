namespace BasicSettings.DataLayer.Repository.Repositories.Contract
{
    public interface IIdentityUserRepository : IRepositoryBase<CustomeIdentityUser>
    {
        List<CustomeIdentityRole> GetRoleById(int? roleId = null);
    }
}
