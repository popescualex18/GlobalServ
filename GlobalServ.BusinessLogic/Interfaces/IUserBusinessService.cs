using GlobalServ.DataAccessLayer.Interfaces;
using GlobalServ.DataModels.Models;
using GlobalServ.DomainModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GlobalServ.BusinessLogic.Interfaces
{
    public interface IUserBusinessService : IBaseBusinessService<UserModel>
    {
        string Register(UserRegistrationDto user);
        LoginResponseDto Login(LoginUserDto loginUser);
        string RefreshToken(RefreshTokenDto refreshToken);
    }
}
