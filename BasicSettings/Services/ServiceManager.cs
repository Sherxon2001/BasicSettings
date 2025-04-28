namespace BasicSettings.Services
{
    public class ServiceManager : IServiceManager
    {
        private readonly IServiceScopeFactory _serviceProvider;

        public ServiceManager(IServiceScopeFactory serviceProvider)
        {
            this._serviceProvider = serviceProvider;
        }

        private TService GetService<TService>()
        {
            var scope = _serviceProvider.CreateScope();
            return scope.ServiceProvider.GetRequiredService<TService>();
        }

        private AppSettings _appsettings;
        public AppSettings Appsettings
        {
            get
            {
                if (_appsettings == null)
                {
                    _appsettings = GetService<AppSettings>();
                }
                return _appsettings;
            }
        }

        private IHelperService _helperService;
        public IHelperService Helper
        {
            get
            {
                if (_helperService == null)
                {
                    _helperService = GetService<IHelperService>();
                }
                return _helperService;
            }
        }

        private ILoggerService _loggerService;
        public ILoggerService LoggerService
        {
            get
            {
                if (_loggerService == null)
                {
                    _loggerService = GetService<ILoggerService>();
                }
                return _loggerService;
            }
        }

        private IntegrationClient _httpClient;
        public IntegrationClient HttpClient
        {
            get
            {
                if (_httpClient == null)
                {
                    _httpClient = GetService<IntegrationClient>();
                }
                return _httpClient;
            }
        }
    }
}
