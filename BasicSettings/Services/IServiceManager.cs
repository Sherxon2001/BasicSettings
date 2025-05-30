using BasicSettings.Services.Client.Concrete;
using BasicSettings.Services.Helper.Concrete;

namespace BasicSettings.Services
{
    public interface IServiceManager
    {
        IntegrationClient HttpClient { get; }
        AppSettings Appsettings { get; }
        IHelperService Helper { get; }
        ILoggerService LoggerService { get; }
    }
}
