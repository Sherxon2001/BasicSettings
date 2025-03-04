namespace BasicSettings.DataLayer.Repository.Repositories
{
    public class CustomeIdentityUserRepository : RepositoryBase<CustomeIdentityUser>, ICustomeIdentityUserRepository
    {
        private readonly IUnitOfWork _unitOfWork;

        public CustomeIdentityUserRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
            this._unitOfWork = unitOfWork;
        }

        public async Task<CustomeIdentityUser> GetUserById(int userId) => await base.FirstOrDefaultAsync(x => x.Id == userId, tracking: false);

        public async Task<CustomeIdentityUser> GetByEmail(string email) => await base.FirstOrDefaultAsync(x => x.NormalizedEmail == email.ToUpper(), tracking: false);

        public async Task<CustomeIdentityUser> GetByUserName(string userName) => await base.FirstOrDefaultAsync(x => x.UserName == userName, tracking: false);

        public async Task<List<CustomeIdentityRole>> GetRolesByUserId(int userId) =>
            await _unitOfWork.Context.UsersRoles.AsNoTracking().Where(x => x.UserId == userId).Select(x => x.Role).ToListAsync();

        public async Task<List<string>> GetSystemTaskByRoleId(int roleId)
        {
            return await _unitOfWork.Context.Set<RoleProfiles>().Where(p => p.RoleId == roleId).Select(p => $"{p.SystemTask.Parent.Name}:{p.SystemTask.Name}").ToListAsync();
        }
    }
}
