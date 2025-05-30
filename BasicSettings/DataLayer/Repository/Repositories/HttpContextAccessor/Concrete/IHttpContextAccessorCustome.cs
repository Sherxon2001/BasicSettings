namespace BasicSettings.DataLayer.Repository.Repositories.HttpContextAccessor.Concrete
{
    public interface IHttpContextAccessorCustome
    {
        IHttpContextAccessor HttpContextAccessor { get; }
        int? GetUserRoleId();
        string GetUserPinFL();
        string GetUserPhoneNumber();
        string GetUserRole();
        long? GetUserId();
        int? GetUserDistrictId();
        bool IsUserActive();
        long? GetUserRegionId();
        string GetUserIPAddress();
        string GetUserAgent();
        string GetUserRemoteAddress();
    }
}
