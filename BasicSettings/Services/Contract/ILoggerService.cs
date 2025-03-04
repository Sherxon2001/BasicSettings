Snamespace BasicSettings.Services.Contract
{
    public interface ILoggerService
    {
        void LogInformation(Exception ex = default, string content = default);
        void LogError(Exception ex = default, string content = default);
        void LogWarning(Exception ex = default, string content = default);
    }
}
