namespace BasicSettings.DataLayer.Repository.Repositories.Concrete
{
    public class RoleProfilesRepository : RepositoryBase<RoleProfiles>, IRoleProfilesRepository
    {
        public RoleProfilesRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }
    }
}
