using BasicSettings.Constants;

namespace BasicSettings.DataLayer.Repository.Repositories.Concrete
{
    public class IdentityUserRepository : RepositoryBase<CustomeIdentityUser>, IIdentityUserRepository
    {
        private readonly IUnitOfWork _unitOfWork;

        public IdentityUserRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
            this._unitOfWork = unitOfWork;
        }

        public List<CustomeIdentityRole> GetRoleById(int? roleId = null)
        {
            var roles = new List<CustomeIdentityRole>();
            if (roleId is null || roleId == AuthConstIds.ROLE_ADMINISTRATOR_ID)
                roles = _unitOfWork.Context.Set<CustomeIdentityRole>().ToList();
            else
                roles = _unitOfWork.Context.Set<CustomeIdentityRole>().Where(x => x.Id > roleId).ToList();
            return roles;
        }
    }
}
