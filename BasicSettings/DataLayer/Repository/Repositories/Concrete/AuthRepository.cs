using BasicSettings.Constants;

namespace BasicSettings.DataLayer.Repository.Repositories.Concrete
{
    public class AuthRepository : RepositoryBase<CustomeIdentityUser>, IAuthRepository
    {
        private readonly IUnitOfWork _unitOfWork;

        public AuthRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
            this._unitOfWork = unitOfWork;
        }

        public List<RoleProfiles> GetRoleProfilesByRoleId(int? roleId)
        {
            var roleProfiles = new List<RoleProfiles>();
            if (roleId is null || roleId == AuthConstIds.ROLE_ADMINISTRATOR_ID)
                roleProfiles = _unitOfWork.Context.Set<RoleProfiles>().Include(x => x.SystemTasks).ToList();
            else
                roleProfiles = _unitOfWork.Context.Set<RoleProfiles>().Where(x => x.RoleId > roleId).Include(x => x.SystemTasks).ToList();
            return roleProfiles;
        }
    }
}
