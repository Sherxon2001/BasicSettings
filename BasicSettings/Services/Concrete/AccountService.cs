namespace BasicSettings.Services.Concrete
{
    public class AccountService : IAccountService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly Appsettings _appsettings;

        public AccountService(IUnitOfWork unitOfWork, Appsettings appsettings)
        {
            this._unitOfWork = unitOfWork;
            this._appsettings = appsettings;
        }

        public async Task<StateModel<TokenDto>> RefreshToken(string refreshToken)
        {
            var _state = StateModel<TokenDto>.Create();
            try
            {
                var userId = _unitOfWork.HttpContextAccessor.GetUserId();

                var user = await _unitOfWork.CustomeIdentityUserRepository.GetUserById(userId);

                var authClaims = await GetAuthClaims(user);
                var jwtSecurityToken = GetJwtSecurityToken(authClaims);
                await SetPermessionKey(user);
                user.RefreshToken = jwtSecurityToken.Claims.First(x => x.Type == JwtRegisteredClaimNames.Jti).Value;
                user.LastActiveDate = DateTime.Now;
                _unitOfWork.CustomeIdentityUserRepository.Update(user);
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

        public async Task<StateModel<TokenDto>> GetToken(LoginDtoI dtoI, CancellationToken cancellationToken)
        {
            var _state = StateModel<TokenDto>.Create();
            try
            {
                var user = await _unitOfWork.CustomeIdentityUserRepository.GetByUserName(dtoI.UserName);
                if (user is null)
                    throw new Exception("User not found!");

                if (!ValidPassword(dtoI, user))
                    throw new Exception("Password is incorrect!");

                if (!user.IsActive || user.LastActiveDate > DateTime.Now.AddMonths(-6))
                    throw new Exception("User is not active!");

                var authClaims = await GetAuthClaims(user);
                var jwtSecurityToken = GetJwtSecurityToken(authClaims);

                await SetPermessionKey(user);

                user.RefreshToken = jwtSecurityToken.Claims.First(x => x.Type == JwtRegisteredClaimNames.Jti).Value;
                user.LastActiveDate = DateTime.Now;
                _unitOfWork.CustomeIdentityUserRepository.Update(user);
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

        private async Task SetPermessionKey(CustomeIdentityUser user)
        {
            var roles = await _unitOfWork.CustomeIdentityUserRepository.GetRolesByUserId(user.Id);

            if (roles.Count == 0)
                throw new Exception("User has no role!");

            foreach (var role in roles)
            {
                var userProfile = _unitOfWork.CacheRepository.GetValueFromCache<int, string>(role.Id);
                if (!string.IsNullOrEmpty(userProfile))
                    continue;

                var userProfiles = await GetPermessionKey(role.Id);
                var timeSpan = TimeSpan.FromMinutes(_appsettings.AuthSettings.Expires);
                _unitOfWork.CacheRepository.SetValueToCache<int, string>(role.Id, userProfiles, timeSpan);
            }
        }

        private async Task<List<Claim>> GetAuthClaims(CustomeIdentityUser user)
        {
            var rolesList = await _unitOfWork.CustomeIdentityUserRepository.GetRolesByUserId(user.Id);
            var roles = string.Empty;
            rolesList.ForEach(x => roles += $"{x}::");
            var refreshToken = Guid.NewGuid().ToString();

            var authClaims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, user.NormalizedUserName),
                    new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                    new Claim(JwtRegisteredClaimNames.Jti, refreshToken),
                    new Claim(AuthConstants.AUTH_USER_ROLES, roles),
                    new Claim(AuthConstants.REGION_ID, user.RegionId?.ToString() ?? string.Empty),
                    new Claim(AuthConstants.DISTRICT_ID, user.DistrictId ?.ToString() ?? string.Empty)
                };
            return authClaims;
        }

        private async Task<string> GetPermessionKey(int userRoleId)
        {
            var userProfiles = string.Empty;
            var umumiy = await _unitOfWork.CustomeIdentityUserRepository.GetSystemTaskByRoleId(userRoleId);
            foreach (var s in umumiy)
                userProfiles += s;
            return userProfiles;
        }

        private JwtSecurityToken GetJwtSecurityToken(List<Claim> authClaims)
        {
            try
            {
                var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_appsettings.AuthSettings.SecretKey));

                var token = new JwtSecurityToken(
                    expires: DateTime.Now.AddMinutes(_appsettings.AuthSettings.Expires),
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
    }
}
