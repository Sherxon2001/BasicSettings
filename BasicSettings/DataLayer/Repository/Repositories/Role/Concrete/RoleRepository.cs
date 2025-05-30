using BasicSettings.DataLayer.Repository.Repositories.Role.Concrete;

namespace BasicSettings.DataLayer.Repository.Repositories.Concrete
{
    public class RoleRepository : RepositoryBase<ApplicantRole>, IRoleRepository
    {
        public RoleRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        { }
    }
}
