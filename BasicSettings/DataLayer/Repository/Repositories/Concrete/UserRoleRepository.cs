namespace BasicSettings.DataLayer.Repository.Repositories.Concrete
{
    public class UserRoleRepository : RepositoryBase<UsersRoles>, IUserRoleRepository
    {
        public UserRoleRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }
    }
}
