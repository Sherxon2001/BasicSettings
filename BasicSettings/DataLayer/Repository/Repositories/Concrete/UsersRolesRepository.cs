namespace BasicSettings.DataLayer.Repository.Repositories.Concrete
{
    public class UsersRolesRepository : RepositoryBase<UsersRoles>, IUsersRolesRepository
    {
        public UsersRolesRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }
    }
}
