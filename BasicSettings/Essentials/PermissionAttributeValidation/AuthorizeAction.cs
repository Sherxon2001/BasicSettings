namespace BasicSettings.Essentials.PermissionAttributeValidation
{
    public class AuthorizeAction : IAuthorizationFilter
    {
        private readonly IServiceManager _services;
        private readonly IUnitOfWork _unitOfWork;

        public AuthorizeAction(IServiceManager services, IUnitOfWork unitOfWork)
        {
            this._services = services;
            this._unitOfWork = unitOfWork;
        }

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            try
            {
                if (context == null)
                {
                    context.Result = new ForbidResult();
                    return;
                }

                var endpoint = context.HttpContext.GetEndpoint();
                if (endpoint?.Metadata?.GetMetadata<IAllowAnonymous>() != null)
                    return;

                var permissionName = GetPermissionName(context);

                if (!AuthorizeAsync(context.HttpContext.User, permissionName))
                {
                    _services.LoggerService.LogError(new Exception($"{permissionName} uchun {_unitOfWork.HttpContextAccessor.GetUserRoleId()} role-da access yo'q"));
                    context.Result = new ForbidResult();
                    return;
                }
            }
            catch (Exception ex)
            {
                _services.LoggerService.LogError(ex);
                context.Result = new StatusCodeResult(StatusCodes.Status401Unauthorized);
            }
        }

        private string GetPermissionName(AuthorizationFilterContext context)
        {
            var controllerName = context.RouteData.Values["controller"]?.ToString();
            var actionName = context.RouteData.Values["action"]?.ToString();
            return $"{controllerName}Controller/{actionName}";
        }


        private bool AuthorizeAsync(ClaimsPrincipal user, string permission)
        {
            try
            {
                int roleId = _unitOfWork.HttpContextAccessor.GetUserRoleId() ?? 0;
                var userProfiles = _unitOfWork.CacheRepository.GetValueFromCache<int, List<string>>(roleId);
                if (userProfiles is not null && userProfiles.Contains(permission))
                    return true;

                else if (userProfiles is null || userProfiles.Count == 0)
                {
                    _services.Helper.UpdatePermissionCheckFromCache(roleId);
                    userProfiles = _unitOfWork.CacheRepository.GetValueFromCache<int, List<string>>(roleId);
                    if (userProfiles is not null && userProfiles.Contains(permission))
                        return true;
                }
                var response1 = false;
                return response1;
            }
            catch (Exception ex)
            {
                _services.LoggerService.LogError(ex);
                var response = false;
                return response;
            }
        }
    }
}
