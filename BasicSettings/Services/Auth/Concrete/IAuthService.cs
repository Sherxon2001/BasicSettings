﻿namespace BasicSettings.Services.Auth.Concrete
{
    public interface IAuthService
    {
        Task<StateModel<TokenDto>> RefreshToken(string refreshToken, CancellationToken cancellationToken);
        Task<StateModel<TokenDto>> GetToken(LoginDtoI dtoI, CancellationToken cancellationToken);
        List<string> GetControlleAction();
        Task CreateActionToRoleProfile(CancellationToken cancellationToken);
        Task<StateModel<bool>> CreateSystemTask(CreateSystemTaskDtoI dtoI);
        Task<StateModel<HashSet<RoleProfileDto>>> GetUserPermissionsWithCheck(CancellationToken cancellationToken);
        Task<StateModel<bool>> UpdateUserPermissionsWithCheck(RoleProfileDtoI dtoI, CancellationToken cancellationToken);
        Task<StateModel<bool>> UpdateSystemTask(CreateSystemTaskDtoI dtoI, CancellationToken cancellationToken);
        Task<StateModel<bool>> DeleteSystemTask(int taskId);
        void UserValidated(ApplicantUser user);

    }
}
