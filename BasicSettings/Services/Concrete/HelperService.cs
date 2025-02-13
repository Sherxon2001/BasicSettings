namespace BasicSettings.Services.Concrete
{
    public class HelperService : IHelperService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly Appsettings _appsettings;

        public HelperService(IUnitOfWork unitOfWork, Appsettings appsettings)
        {
            this._unitOfWork = unitOfWork;
            this._appsettings = appsettings;
        }


    }
}
