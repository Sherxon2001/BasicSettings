namespace BasicSettings.Services
{
    public interface IServiceManager
    {
        IntegrationClient HttpClient { get; }
        Appsettings Appsettings { get; }
        IHelperService Helper { get; }
        ILoggerService LoggerService { get; }
    }
}
