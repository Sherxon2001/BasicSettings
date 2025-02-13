using BasicSettings.Services.Contract;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
            _logger.LogError($"{content} {ex?.Message}");
        }

        public void LogWarning(Exception ex = default, string content = default)
        {
            _logger.LogWarning($"{content} {ex?.Message}");
        }
    }
}
