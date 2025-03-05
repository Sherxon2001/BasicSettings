namespace BasicSettings.DataLayer.Repository.Repositories.Concrete
{
    public class HttpContextAccessorCustome : IHttpContextAccessorCustome
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private ClaimsPrincipal _user;
        public HttpContextAccessorCustome(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
            _user = _httpContextAccessor?.HttpContext?.User;
        }

        public IHttpContextAccessor HttpContextAccessor => _httpContextAccessor;

        public int? GetUserRoleId()
        {
            if (!_user?.Identity?.IsAuthenticated ?? true)
                return null;

            if (Int32.TryParse(_user?.FindFirst(AuthConstIds.USER_ROLE_ID)?.Value, out int i))
                return i;
            return null;
        }

        public string GetUserPinFL()
        {
            if (!_user?.Identity?.IsAuthenticated ?? true)
                return string.Empty;

            var pinFl = _user?.FindFirst(AuthConstIds.USER_PINF)?.Value;
            return pinFl;
        }

        public string GetUserPhoneNumber()
        {
            if (!_user?.Identity?.IsAuthenticated ?? true)
                return string.Empty;

            var phoneNumber = _user?.FindFirst(AuthConstIds.USER_PHONE_NUMBER)?.Value;

            return phoneNumber;
        }

        public string GetUserRole()
        {
            if (!_user?.Identity?.IsAuthenticated ?? true)
                return string.Empty;

            var role = _user?.FindFirst(AuthConstIds.USER_ROLE)?.Value;
            return role;
        }

        public long? GetUserId()
        {
            if (!_user?.Identity?.IsAuthenticated ?? true)
                return null;

            if (Int64.TryParse(_user?.FindFirst(AuthConstIds.USER_ID)?.Value, out Int64 i))
                return i;
            return null;
        }

        public int? GetUserDistrictId()
        {
            if (!_user?.Identity?.IsAuthenticated ?? true)
                return null;

            if (Int32.TryParse(_user?.FindFirst(AuthConstIds.USER_DISTRICT_ID)?.Value, out Int32 i))
                return i;
            return null;
        }

        public bool IsUserActive()
        {
            if (!_user?.Identity?.IsAuthenticated ?? true)
                return false;

            if (bool.TryParse(_user?.FindFirst(AuthConstIds.USER_IS_ACTIVE)?.Value, out bool i))
                return i;
            return false;
        }

        public long? GetUserRegionId()
        {
            if (!_user?.Identity?.IsAuthenticated ?? false)
                return 0;

            if (Int64.TryParse(_user?.FindFirst(AuthConstIds.USER_REGION_ID)?.Value, out Int64 i))
                return i;
            return null;
        }

        public string GetUserIPAddress()
        {
            Ping ping = new Ping();
            var replay = ping.Send(Dns.GetHostName());

            if (replay.Status == IPStatus.Success)
            {
                return replay.Address.ToString();
            }
            return string.Empty;
        }

        public string GetUserAgent()
        {
            return _httpContextAccessor.HttpContext?.Request?.Headers["User-Agent"].ToString();
        }

        public string GetUserRemoteAddress()
        {
            return _httpContextAccessor.HttpContext?.Connection?.RemoteIpAddress?.ToString();
        }
    }
}
