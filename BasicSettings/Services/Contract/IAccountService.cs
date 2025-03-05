using BasicSettings.Models.Additional;

namespace BasicSettings.Services.Contract
{
    public interface IAccountService
    {
        Task<StateModel<TokenDto>> RefreshToken(string refreshToken);
        Task<StateModel<TokenDto>> GetToken(LoginDtoI dtoI, CancellationToken cancellationToken);
    }
}
