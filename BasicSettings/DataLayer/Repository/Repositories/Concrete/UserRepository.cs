namespace BasicSettings.DataLayer.Repository.Repositories.Concrete
{
    public class UserRepository : RepositoryBase<CustomeIdentityUser>, IUserRepository
    {
        private readonly IUnitOfWork _unitOfWork;

        public UserRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<CustomeIdentityUser> GetUserById(long userId) => await FirstOrDefaultAsync(x => x.Id == userId, tracking: false);

        public async Task<CustomeIdentityUser> GetByEmail(string email) => await FirstOrDefaultAsync(x => x.NormalizedEmail == email.ToUpper(), tracking: false);

        public async Task<CustomeIdentityUser> GetByUserName(string userName) => await FirstOrDefaultAsync(x => x.UserName == userName, tracking: false);

        public async Task<List<CustomeIdentityRole>> GetRolesByUserId(long userId) =>
            await _unitOfWork.Context.UsersRoles.AsNoTracking().Where(x => x.UserId == userId).Select(x => x.Role).ToListAsync();

        public async Task<List<string>> GetSystemTaskByRoleId(int roleId)
        {
            return await _unitOfWork.Context.Set<RoleProfiles>().Where(p => p.RoleId == roleId).Select(p => $"{p.SystemTasks.Parent.Name}:{p.SystemTasks.Name}").ToListAsync();
        }
    }
}
