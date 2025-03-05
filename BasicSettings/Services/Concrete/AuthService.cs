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

        public async Task<StateModel<TokenDto>> GetToken(LoginDtoI dtoI, CancellationToken cancellationToken)
        {
            var _state = StateModel<TokenDto>.CreateInstance();
            try
            {
                var user = await _unitOfWork.UserRepository.GetByUserName(dtoI.UserName);
                if (user is null || !user.IsActive || user.LastActiveDate > DateTime.Now.AddMonths(-6))
                    throw new Exception("User is not active!");

                if (!ValidPassword(dtoI, user))
                    throw new Exception("Password is incorrect!");

                var authClaims = await GetAuthClaims(user);
                var jwtSecurityToken = GetJwtSecurityToken(authClaims);

                await SetPermessionKey(user);

                user.RefreshToken = jwtSecurityToken.Claims.First(x => x.Type == JwtRegisteredClaimNames.Jti).Value;
                user.LastActiveDate = DateTime.Now;
                _unitOfWork.UserRepository.Update(user);
                await _unitOfWork.SaveAsync(cancellationToken);

                var token = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken);

                _state.SetMessage(ConstIds.SUCCESSFULLY);
                _state.SetCode(StatusCodes.Status200OK);
                _state.SetData(new TokenDto
                {
                    Token = token,
                    ExpirationDate = jwtSecurityToken.ValidTo,
                    RefreshToken = user.RefreshToken,
                    User = new CustomeIdentityUserDto
                    {
                        Id = user.Id,
                        UserName = user.UserName,
                        Email = user.Email,
                        PhoneNumber = user.PhoneNumber,
                        IsActive = user.IsActive,
                        RegionId = user.RegionId,
                        DistrictId = user.DistrictId
                    }
                });
            }
            catch (Exception ex)
            {
                _state.SetCode(StatusCodes.Status400BadRequest);
                _state.SetMessage(ex.Message);
            }
            return _state;
        }

        public async Task<StateModel<TokenDto>> RefreshToken(string refreshToken)
        {
            var _state = StateModel<TokenDto>.CreateInstance();
            try
            {
                var userId = _unitOfWork.HttpContextAccessor.GetUserId() ?? 0;

                var user = await _unitOfWork.UserRepository.GetUserById(userId);

                var authClaims = await GetAuthClaims(user);
                var jwtSecurityToken = GetJwtSecurityToken(authClaims);
                await SetPermessionKey(user);
                user.RefreshToken = jwtSecurityToken.Claims.First(x => x.Type == JwtRegisteredClaimNames.Jti).Value;
                user.LastActiveDate = DateTime.Now;
                _unitOfWork.UserRepository.Update(user);
                await _unitOfWork.SaveAsync();
                var token = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken);
                _state.SetMessage(ConstIds.SUCCESSFULLY);
                _state.SetCode(StatusCodes.Status200OK);
                _state.SetData(new TokenDto
                {
                    Token = token,
                    ExpirationDate = jwtSecurityToken.ValidTo,
                    RefreshToken = user.RefreshToken,
                    User = new CustomeIdentityUserDto
                    {
                        Id = user.Id,
                        UserName = user.UserName,
                        Email = user.Email,
                        PhoneNumber = user.PhoneNumber,
                        IsActive = user.IsActive,
                        RegionId = user.RegionId,
                        DistrictId = user.DistrictId
                    }
                });
            }
            catch (Exception ex)
            {
                _state.SetCode(StatusCodes.Status400BadRequest);
                _state.SetMessage(ex.Message);
            }
            return _state;
        }

        private async Task SetPermessionKey(CustomeIdentityUser user)
        {
            var roles = await _unitOfWork.UserRepository.GetRolesByUserId(user.Id);

            if (roles.Count == 0)
                throw new Exception("User has no role!");

            foreach (var role in roles)
            {
                var userProfile = _unitOfWork.CacheRepository.GetValueFromCache<int, string>(role.Id);
                if (!string.IsNullOrEmpty(userProfile))
                    continue;

                var userProfiles = await GetPermessionKey(role.Id);
                var timeSpan = TimeSpan.FromMinutes(_service.Appsettings.AuthSettings.Expires);
                _unitOfWork.CacheRepository.SetValueToCache<int, string>(role.Id, userProfiles, timeSpan);
            }
        }

        private async Task<List<Claim>> GetAuthClaims(CustomeIdentityUser user)
        {
            var rolesList = await _unitOfWork.UserRepository.GetRolesByUserId(user.Id);
            var roles = string.Empty;
            rolesList.ForEach(x => roles += $"{x}::");
            var refreshToken = Guid.NewGuid().ToString();

            var authClaims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, user.NormalizedUserName),
                    new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                    new Claim(JwtRegisteredClaimNames.Jti, refreshToken),
                    new Claim(AuthConstIds.USER_ROLES, roles),
                    new Claim(AuthConstIds.USER_REGION_ID, user.RegionId?.ToString() ?? string.Empty),
                    new Claim(AuthConstIds.USER_DISTRICT_ID, user.DistrictId ?.ToString() ?? string.Empty)
                };
            return authClaims;
        }

        private async Task<string> GetPermessionKey(int userRoleId)
        {
            var userProfiles = string.Empty;
            var umumiy = await _unitOfWork.UserRepository.GetSystemTaskByRoleId(userRoleId);
            foreach (var s in umumiy)
                userProfiles += s;
            return userProfiles;
        }

        private JwtSecurityToken GetJwtSecurityToken(List<Claim> authClaims)
        {
            try
            {
                var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_service.Appsettings.AuthSettings.SecretKey));
                var token = new JwtSecurityToken(
                    expires: DateTime.Now.AddMinutes(_service.Appsettings.AuthSettings.Expires),
                    claims: authClaims,
                    signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
                    );
                return token;
            }
            catch (Exception ex)
            {
                return new JwtSecurityToken();
            }
        }

        private bool ValidPassword(LoginDtoI login, CustomeIdentityUser user)
        {
            var passwordHash = GeneratePasswordHash(login.UserName, login.Password, user.SecurityStamp);

            return passwordHash == user.PasswordHash;
        }

        private string GeneratePasswordHash(string username, string password, string securityStamp)
        {
            var sekret = CreateHmacSha256Key(username.ToUpper(), securityStamp);

            return ComputeHmacSha256Hash(sekret, password);
        }

        private static byte[] CreateHmacSha256Key(string key, string message)
        {
            using var hmac = new HMACSHA256(Encoding.UTF8.GetBytes(message));
            return hmac.ComputeHash(Encoding.UTF8.GetBytes(key));
        }

        private static string ComputeHmacSha256Hash(byte[] key, string data)
        {
            using var hmac = new HMACSHA256(key);
            var hash = hmac.ComputeHash(Encoding.UTF8.GetBytes(data));
            return BitConverter.ToString(hash).Replace("-", "").ToLower();
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
