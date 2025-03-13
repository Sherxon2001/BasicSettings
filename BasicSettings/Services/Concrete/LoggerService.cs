namespace BasicSettings.Services.Concrete
{
    public class LoggerService : ILoggerService
    {
        private readonly ILogger _logger;
        public LoggerService(ILogger logger)
        {
            _logger = logger;
        }

        public void LogInformation(Exception ex = default, string content = default)
        {
            _logger.LogInformation($"{content} {ex?.Message}");
        }

        public void LogError(Exception ex = default, string content = default)
        {
            var className = GetClassName(ex);
            var methodName = GetMethodName(ex);
            var message = $"\n{ex.Message}\nClass: {className}\nMethod: {methodName}\n{ex.InnerException}";

            try
            {
                //_service.TelegramBotClient.SendMessage(757851321, message);
            }
            catch { }
            _logger.LogError($"{content} {message}");
        }

        public void LogWarning(Exception ex = default, string content = default)
        {
            _logger.LogWarning($"{content} {ex?.Message}");
        }

        private string GetClassName(Exception ex)
        {
            var st = new StackTrace(ex, true);
            var frame = st.GetFrame(0);
            var method = frame.GetMethod();
            return method.DeclaringType.FullName;
        }

        private string GetMethodName(Exception ex)
        {
            var st = new StackTrace(ex, true);
            var frame = st.GetFrame(0);
            var method = frame.GetMethod();
            return method.Name;
        }
    }
}
