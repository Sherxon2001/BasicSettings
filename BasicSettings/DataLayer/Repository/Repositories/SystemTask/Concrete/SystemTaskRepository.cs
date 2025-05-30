namespace BasicSettings.DataLayer.Repository.Repositories.SystemTask.Concrete
{
    public class SystemTaskRepository : RepositoryBase<SystemTasks>, ISystemTaskRepository
    {
        private readonly IUnitOfWork _unitOfWork;

        public SystemTaskRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
    }
}
