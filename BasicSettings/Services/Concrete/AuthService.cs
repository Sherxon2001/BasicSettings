namespace BasicSettings.Services.Concrete
{
    public class AuthService : IAuthService
    {
        private readonly IServiceManager _service;
        private readonly IUnitOfWork _unitOfWork;

        public AuthService(IServiceManager service, IUnitOfWork unitOfWork)
        {
            this._service = service;
            this._unitOfWork = unitOfWork;
        }

        public List<string> GetControlleAction()
        {
            /*var assemblies = AppDomain.GetAssemblies();

            var controllers = assemblies.SelectMany(assembly => assembly.GetTypes())
                                        .Where(type => typeof(ControllerBase).IsAssignableFrom(type) && !type.IsAbstract)
                                        .ToList();
            var controllerMethods = new List<string>();

            foreach (var controller in controllers)
            {
                var methods = controller.GetMethods(BindingFlags.Instance | BindingFlags.Public | BindingFlags.DeclaredOnly)
                                        .Where(method => !method.IsSpecialName)
                                        .Select(method => method.Name)
                                        .ToList();
                foreach (var method in methods)
                {
                    controllerMethods.Add($"{controller.Name}/{method}");
                }
            }
            return controllerMethods;*/
            var assemblies = AppDomain.CurrentDomain.GetAssemblies();
            var controllerMethods = new List<string>();

            foreach (var assembly in assemblies)
            {
                try
                {
                    var types = assembly.GetTypes();
                    var controllers = types.Where(type => typeof(ControllerBase).IsAssignableFrom(type) && !type.IsAbstract).ToList();
                    foreach (var controller in controllers)
                    {
                        var methods = controller.GetMethods(BindingFlags.Instance | BindingFlags.Public | BindingFlags.DeclaredOnly)
                                                .Where(method => !method.IsSpecialName)
                                                .Select(method => method.Name)
                                                .ToList();
                        foreach (var method in methods)
                        {
                            controllerMethods.Add($"{controller.Name}/{method}");
                        }
                    }
                }
                catch (ReflectionTypeLoadException ex)
                {
                    _service.LoggerService.LogError(ex);
                }
            }
            return controllerMethods;
        }

        public async Task CreateActionToRoleProfile(CancellationToken cancellationToken)
        {
            try
            {
                var controllerMethods = GetControlleAction();

                if (controllerMethods.Count == 0)
                    return;

                var tasks = await _unitOfWork.SystemTaskRepository.AsQueryable().ToListAsync(cancellationToken);
                var otherTask = tasks.FirstOrDefault(x => x.ActionName.Equals("Other"));

                List<SystemTasks> systemTasks = new List<SystemTasks>();
                foreach (var item in controllerMethods)
                {
                    if (!tasks.Any(x => x.ActionName.Equals(item)))
                    {
                        var task = new SystemTasks
                        {
                            ParentId = otherTask.Id,
                            Name = item,
                            OrderBy = 1,
                            Type = AuthConstIds.SYSTEM_TASKS_ACTION,
                            ActionName = item,
                        };
                        systemTasks.Add(task);
                    }
                }

                if (systemTasks.Any())
                {
                    await _unitOfWork.SystemTaskRepository.AddRangeAsync(systemTasks, cancellationToken);
                    await _unitOfWork.SaveChangesAsync(cancellationToken);
                }

                var adminTasks = await _unitOfWork.RoleProfilesRepository.Where(x => x.RoleId == AuthConstIds.ROLE_ADMINISTRATOR_ID).ToListAsync(cancellationToken);
                var adminTaskIds = adminTasks.Select(x => x.TaskId).ToList();

                var freeTasks = _unitOfWork.SystemTaskRepository.Where(x => !adminTaskIds.Contains(x.Id)).Select(x => new RoleProfiles { RoleId = AuthConstIds.ROLE_ADMINISTRATOR_ID, TaskId = x.Id }).ToList();
                if (freeTasks.Any())
                {
                    await _unitOfWork.RoleProfilesRepository.AddRangeAsync(freeTasks, cancellationToken);
                    await _unitOfWork.SaveChangesAsync(cancellationToken);
                }
            }
            catch (Exception ex)
            {
                _service.LoggerService.LogError(ex);
            }
        }

        public async Task<StateModel<bool>> CreateSystemTask(CreateSystemTaskDtoI dtoI)
        {
            try
            {
                var chechAction = await _unitOfWork.SystemTaskRepository.FirstOrDefaultAsync(x => x.Name.ToUpper() == dtoI.Name.ToUpper() || x.ActionName.ToUpper() == dtoI.Name.ToUpper());
                if (chechAction is not null)
                    return StateModel<bool>.CreateInstance(StatusCodes.Status400BadRequest, "Bunday Task mavjud.");

                var task = new SystemTasks
                {
                    ParentId = dtoI.ParentId,
                    Name = dtoI.Name,
                    OrderBy = dtoI.Order,
                    Type = dtoI.Type,
                    ActionName = dtoI.Name
                };

                await _unitOfWork.SystemTaskRepository.AddAsycn(task);
                await _unitOfWork.SaveChangesAsync();

                return StateModel<bool>.CreateInstance(StatusCodes.Status200OK, ConstIds.SUCCESSFULLY, true);
            }
            catch (Exception ex)
            {
                _service.LoggerService.LogError(ex);
                return StateModel<bool>.CreateInstance(StatusCodes.Status400BadRequest, ex.Message);
            }
        }

        public async Task<StateModel<HashSet<RoleProfileDto>>> GetUserPermissionsWithCheck(CancellationToken cancellationToken)
        {
            var _state = StateModel<HashSet<RoleProfileDto>>.CreateInstance();
            try
            {
                await CreateActionToRoleProfile(cancellationToken);

                var roles = _unitOfWork.IdentityUserRepository.GetRoleById()
                    .Select(x =>
                    new RoleProfileDto
                    {
                        Id = x.Id,
                        Name = x.Name
                    });

                var menus = _unitOfWork.SystemTaskRepository.AsQueryable(includes: x => x.Children)
                    .Where(x => x.ParentId.Equals(null) || x.Type.Equals(AuthConstIds.SYSTEM_TASKS_MENU))
                    .Select(x => new RoleProfileMenuDto
                    {
                        Id = x.Id,
                        Name = x.Name,
                        SubMenus = x.Children.Where(y => y.Type.Equals(AuthConstIds.SYSTEM_TASKS_SUB_MENU)).Select(y => new RoleProfileSubMenuDto
                        {
                            Id = y.Id,
                            ParentId = y.ParentId,
                            Name = y.Name
                        }).ToHashSet(),
                        Actions = x.Children.Where(y => y.Type.Equals(AuthConstIds.SYSTEM_TASKS_ACTION)).Select(y => new RoleProfileActionDto
                        {
                            Id = y.Id,
                            ParentId = y.ParentId,
                            Name = y.Name
                        }).ToHashSet()
                    }).ToHashSet();

                var roleProfiles = await _unitOfWork.RoleProfilesRepository.AsQueryable().ToListAsync(cancellationToken);

                var rolesV2 = new HashSet<RoleProfileDto>();
                foreach (var role in roles)
                {
                    var rol2 = new RoleProfileDto
                    {
                        Id = role.Id,
                        Name = role.Name,
                    };
                    foreach (var menu in menus)
                    {
                        var men2 = new RoleProfileMenuDto
                        {
                            Id = menu.Id,
                            Name = menu.Name,
                            IsActive = roleProfiles.Any(x => x.TaskId.Equals(menu.Id) && x.RoleId.Equals(role.Id)) ? true : false
                        };

                        foreach (var sub_menu in menu.SubMenus)
                        {
                            var sub2 = new RoleProfileSubMenuDto
                            {
                                ParentId = sub_menu.ParentId,
                                Id = sub_menu.Id,
                                Name = sub_menu.Name,
                                IsActive = roleProfiles.Any(x => x.TaskId.Equals(sub_menu.Id) && x.RoleId.Equals(role.Id)) ? true : false
                            };
                            men2.SubMenus.Add(sub2);
                        }
                        foreach (var action in menu.Actions)
                        {
                            var action2 = new RoleProfileActionDto
                            {
                                ParentId = action.ParentId,
                                Id = action.Id,
                                Name = action.Name,
                                IsActive = roleProfiles.Any(x => x.TaskId.Equals(action.Id) && x.RoleId.Equals(role.Id)) ? true : false
                            };
                            men2.Actions.Add(action2);
                        }
                        rol2.Menus.Add(men2);
                    }
                    rolesV2.Add(rol2);

                }

                return StateModel<HashSet<RoleProfileDto>>.CreateInstance(StatusCodes.Status200OK, ConstIds.SUCCESSFULLY, rolesV2);
            }
            catch (Exception ex)
            {
                _service.LoggerService.LogError(ex);
                return StateModel<HashSet<RoleProfileDto>>.CreateInstance(StatusCodes.Status400BadRequest, ex.Message);
            }
        }

        public async Task<StateModel<bool>> UpdateUserPermissionsWithCheck(RoleProfileDtoI dtoI, CancellationToken cancellationToken)
        {
            var _state = StateModel<bool>.CreateInstance();
            try
            {
                await _unitOfWork.ExecuteInTransactionAsync(async () =>
                {
                    _unitOfWork.RoleProfilesRepository.RemoveRange(_unitOfWork.RoleProfilesRepository.AsQueryable().Where(x => x.RoleId == dtoI.RoleId));
                    await _unitOfWork.SaveChangesAsync();
                    var roleProfiles = new List<RoleProfiles>();
                    foreach (var taskId in dtoI.TaskIds.Distinct())
                    {
                        var roleProfile = new RoleProfiles
                        {
                            TaskId = taskId,
                            RoleId = dtoI.RoleId
                        };
                        roleProfiles.Add(roleProfile);
                    }
                    await _unitOfWork.RoleProfilesRepository.AddRangeAsync(roleProfiles, cancellationToken);
                    await _unitOfWork.SaveChangesAsync(cancellationToken);
                });
                return StateModel<bool>.CreateInstance(StatusCodes.Status200OK, ConstIds.SUCCESSFULLY, true);
            }
            catch (Exception ex)
            {
                _service.LoggerService.LogError(ex);
                return StateModel<bool>.CreateInstance(StatusCodes.Status400BadRequest, ex.Message);
            }
        }

        public async Task<StateModel<bool>> UpdateSystemTask(CreateSystemTaskDtoI dtoI, CancellationToken cancellationToken)
        {
            var _state = new StateModel<bool>();
            try
            {
                var checkTask = await _unitOfWork.SystemTaskRepository.FirstOrDefaultAsync(x => x.Id == dtoI.Id, tracking: true);
                if (checkTask is null)
                    return StateModel<bool>.CreateInstance(StatusCodes.Status400BadRequest, "Bunday TaskId yo'q.");

                checkTask.ParentId = dtoI.ParentId;
                checkTask.Name = dtoI.Name;
                checkTask.OrderBy = dtoI.Order;
                checkTask.Type = dtoI.Type;

                await _unitOfWork.SaveChangesAsync(cancellationToken);
                return StateModel<bool>.CreateInstance(StatusCodes.Status200OK, ConstIds.SUCCESSFULLY, true);
            }
            catch (Exception ex)
            {
                _service.LoggerService.LogError(ex);
                return StateModel<bool>.CreateInstance(StatusCodes.Status400BadRequest, ex.Message);
            }
        }

        public async Task<StateModel<bool>> DeleteSystemTask(int taskId)
        {
            var _state = new StateModel<bool>();
            try
            {
                var checkTask = await _unitOfWork.SystemTaskRepository.FirstOrDefaultAsync(x => x.Id == taskId);
                if (checkTask is null)
                    return StateModel<bool>.CreateInstance(StatusCodes.Status400BadRequest, "Bunday TaskId yo'q.");

                await _unitOfWork.ExecuteInTransactionAsync(async () =>
                {
                    _unitOfWork.RoleProfilesRepository.RemoveRange(_unitOfWork.RoleProfilesRepository.Where(x => x.TaskId == taskId));
                    _unitOfWork.SystemTaskRepository.Remove(checkTask);
                    await _unitOfWork.SaveChangesAsync();
                });

                return StateModel<bool>.CreateInstance(StatusCodes.Status200OK, ConstIds.SUCCESSFULLY, true);
            }
            catch (Exception ex)
            {
                _service.LoggerService.LogError(ex);
                return StateModel<bool>.CreateInstance(StatusCodes.Status400BadRequest, ex.Message);
            }
        }
    }
}
