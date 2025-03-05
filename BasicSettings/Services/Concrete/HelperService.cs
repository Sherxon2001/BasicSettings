namespace BasicSettings.Services.Concrete
{
    public class HelperService : IHelperService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IServiceManager _service;

        public HelperService(IUnitOfWork unitOfWork, IServiceManager service)
        {
            this._unitOfWork = unitOfWork;
            this._service = service;
        }

        public void UpdatePermissionCheckFromCache(int roleId)
        {
            var value = _unitOfWork.CacheRepository.GetValueFromCache<int, List<string>>(roleId);

            if (value?.Count() > 0)
                return;

            var systemTasks = _unitOfWork.IdentityUserRepository.GetRoleProfilesByRoleId(roleId).Select(x => x.SystemTasks.ActionName).ToList();

            if (systemTasks?.Count == 0 || systemTasks is null)
                return;

            _unitOfWork.CacheRepository.SetValueToCache<int, List<string>>(roleId, systemTasks, TimeSpan.FromMinutes(_service.Appsettings.AuthSettings.Expires));
        }
    }
}
