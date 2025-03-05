namespace BasicSettings.Services.Contract
{
    public interface IAuthService
    {
        List<string> GetControlleAction();
        Task CreateActionToRoleProfile(CancellationToken cancellationToken);
        Task<StateModel<bool>> CreateSystemTask(CreateSystemTaskDtoI dtoI);
        Task<StateModel<HashSet<RoleProfileDto>>> GetUserPermissionsWithCheck(CancellationToken cancellationToken);
        Task<StateModel<bool>> UpdateUserPermissionsWithCheck(RoleProfileDtoI dtoI, CancellationToken cancellationToken);
        Task<StateModel<bool>> UpdateSystemTask(CreateSystemTaskDtoI dtoI, CancellationToken cancellationToken);
        Task<StateModel<bool>> DeleteSystemTask(int taskId);

    }
}
