namespace BasicSettings.DataLayer.Repository.Repositories.Contract
{
    public interface IAuthRepository : IRepositoryBase<CustomeIdentityUser>
    {
        List<RoleProfiles> GetRoleProfilesByRoleId(int? roleId);
    }
}
