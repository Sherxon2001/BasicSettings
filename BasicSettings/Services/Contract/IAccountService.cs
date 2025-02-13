using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BasicSettings.Services.Contract
{
    public interface IAccountService
    {
        Task<StateModel<TokenDto>> RefreshToken(string refreshToken);
        Task<StateModel<TokenDto>> GetToken(LoginDtoI dtoI, CancellationToken cancellationToken);
    }
}
