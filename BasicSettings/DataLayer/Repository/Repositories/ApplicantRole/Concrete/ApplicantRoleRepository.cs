namespace BasicSettings.DataLayer.Repository.Repositories.ApplicantRole.Concrete
{
    public class ApplicantRoleRepository : RepositoryBase<ApplicantRole>, IApplicantRoleRepository
    {
        private readonly IUnitOfWork _unitOfWork;

        public ApplicantRoleRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
    }
}
