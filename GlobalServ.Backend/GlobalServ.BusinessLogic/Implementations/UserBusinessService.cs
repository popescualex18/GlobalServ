using Azure.Core;
using GlobalServ.BusinessLogic.Interfaces;
using GlobalServ.Common.Enum;
using GlobalServ.Core.Helpers;
using GlobalServ.Core.Implementations;
using GlobalServ.Core.Interfaces;
using GlobalServ.DataAccessLayer.Interfaces;
using GlobalServ.DataModels.Models;
using GlobalServ.DomainModels;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GlobalServ.BusinessLogic.Implementations
{
    public class UserBusinessService : BaseBusinessService<UserModel>, IUserBusinessService
    {
        private readonly IGeneratePasswordHelper _generatePasswordHelper;
        private readonly IOptions<ApiOptions> _apiOptions;
        private readonly ITokenHelper _tokenHelper;
        public UserBusinessService(IGenericRepository<UserModel> generic, IGeneratePasswordHelper generatePasswordHelper, IOptions<ApiOptions> options, ITokenHelper tokenHelper) : base(generic)
        {
            _generatePasswordHelper = generatePasswordHelper;
            _apiOptions = options;
            _tokenHelper = tokenHelper;
        }

        public LoginResponseDto Login(LoginUserDto loginUser)
        {
            var response = new LoginResponseDto();
            var loggedUser = GenericRepository.Get(x => x.Email == loginUser.Email).FirstOrDefault();
            if (loggedUser == null)
            {
                response.Error = "Emailul nu exista.";
                return response;
            }

            var password = _generatePasswordHelper.GetHash(loginUser.Password + loggedUser.PasswordSalt);

            // Do not disclose information about account existence.
            // Return 404 Not Found if account doesn't exists or password doesn't match.
            if (password != loggedUser.PasswordHash)
            {
                response.Error = "Parola nu este corecta.";
                return response;
            }


            var refreshToken = GenerateRefreshToken(loggedUser);
            response.AccessToken = _tokenHelper.GenerateToken(
                loggedUser.Id.ToString(),
                loggedUser.Email,
                loggedUser.RoleId.ToString(),
                DateTime.UtcNow.AddHours(_apiOptions.Value.TokenOptions.HoursUntilExpiration),
                _apiOptions.Value.TokenOptions.Secret
            );
            response.RefreshToken = refreshToken;
            response.Role = loggedUser.RoleId.ToString();
            response.FullName = loggedUser.FullName;
            return response;
        }

        public string RefreshToken(RefreshTokenDto refreshToken)
        {
            var principal = _tokenHelper.GetPrincipalFromToken(refreshToken.Token, _apiOptions.Value.TokenOptions.Secret, true);
            var user = GenericRepository.Get(x => x.Id.ToString() == principal.Identity!.Name).FirstOrDefault();
            if (user == null)
            {
                return null;
            }

            return _tokenHelper.GenerateToken(
                user.Id.ToString(),
                user.Email,
                user.RoleId.ToString(),
                DateTime.UtcNow.AddHours(_apiOptions.Value.TokenOptions.HoursUntilExpiration),
                _apiOptions.Value.TokenOptions.Secret
            );
        }

        public string Register(UserRegistrationDto model)
        {
            var existingUser = GenericRepository.Get(x => x.Email == model.Email).FirstOrDefault();
            if (existingUser != null)
            {
                return "Email este folosit. Va rugam folositi alta adresa de email";
            }

            var userToAdd = new UserModel()
            {
                Email = model.Email,
                FullName = model.SurName,
                PhoneNumber = model.PhoneNumber,
                Name = model.Name,
                RoleId = (int)RolesEnum.User,
            };
            var salt = _generatePasswordHelper.GetSalt();

            userToAdd.PasswordSalt = salt;
            userToAdd.PasswordHash = _generatePasswordHelper.GetHash(model.Password + salt);
            Add(userToAdd);
            return string.Empty;
        }
        private string GenerateRefreshToken(UserModel user)
        {
            var tokenHelp = new TokenHelper();
            var refreshTokenExpirationDate = DateTime.UtcNow
                .AddHours(_apiOptions.Value.RefreshTokenOptions.HoursUntilExpiration);

            var refreshToken = tokenHelp.GenerateToken(
                user.Id.ToString(),
                user.Email,
                user.RoleId.ToString(),
                refreshTokenExpirationDate,
                _apiOptions.Value.RefreshTokenOptions.Secret);

            return refreshToken;
        }
    }
}
