using BasicSettings.Models.Additional;

namespace BasicSettings.Services
{
    public interface IServiceManager
    {
        Appsettings Appsettings { get; }
        IHelperService Helper { get; }
        IAccountService AccountService { get; }
        ILoggerService LoggerService { get; }
    }
}
