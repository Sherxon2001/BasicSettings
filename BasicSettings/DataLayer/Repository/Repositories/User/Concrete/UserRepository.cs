namespace BasicSettings.DataLayer.Repository.Repositories.User.Concrete
{
    public class UserRepository : RepositoryBase<ApplicantUser>, IUserRepository
    {
        private readonly IUnitOfWork _unitOfWork;

        public UserRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
    }
}
