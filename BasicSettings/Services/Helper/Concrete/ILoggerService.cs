namespace BasicSettings.Services.Helper.Concrete
{
    public interface ILoggerService
    {
        void LogInformation(Exception ex = default, string content = default);
        void LogError(Exception ex = default, string content = default);
        void LogWarning(Exception ex = default, string content = default);
    }
}
