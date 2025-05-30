namespace BasicSettings.Services.Helper.Concrete
{
    public class HelperService : IHelperService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IServiceManager _service;

        public HelperService(IUnitOfWork unitOfWork, IServiceManager service)
        {
            _unitOfWork = unitOfWork;
            _service = service;
        }

        public void UpdatePermissionCheckFromCache(int roleId)
        {
            var value = _unitOfWork.CacheRepository.GetValueFromCache<int, List<string>>(roleId);

            if (value?.Count() > 0)
                return;

            var systemTasks = _unitOfWork.RoleProfilesRepository.Where(x => x.RoleId == roleId).Select(x => x.SystemTasks.ActionName).ToList();

            if (systemTasks?.Count == 0 || systemTasks is null)
                return;

            _unitOfWork.CacheRepository.SetValueToCache<int, List<string>>(roleId, systemTasks, TimeSpan.FromMinutes(_service.Appsettings.AuthSettings.TokenExpires));
        }
    }
}
