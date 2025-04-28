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
