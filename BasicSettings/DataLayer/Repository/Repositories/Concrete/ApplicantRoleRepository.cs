namespace BasicSettings.DataLayer.Repository.Repositories.Concrete
{
    public class ApplicantRoleRepository : RepositoryBase<ApplicantRole>, IApplicantRoleRepository
    {
        private readonly IUnitOfWork _unitOfWork;

        public ApplicantRoleRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
            this._unitOfWork = unitOfWork;
        }
    }
}
