namespace BasicSettings.DataLayer.Repository.Repositories.Concrete
{
    public class SystemTaskRepository : RepositoryBase<SystemTasks>, ISystemTaskRepository
    {
        private readonly IUnitOfWork _unitOfWork;

        public SystemTaskRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
            this._unitOfWork = unitOfWork;
        }
    }
}
