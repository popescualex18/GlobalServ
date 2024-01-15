using GlobalServ.BusinessLogic.Interfaces;
using GlobalServ.Common.Enum;
using GlobalServ.DataModels.Models;
using GlobalServ.DomainModels;
using Microsoft.AspNetCore.Authorization.Infrastructure;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Reflection.Metadata.Ecma335;

namespace GlobalServ.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserBusinessService _userBusinessService;
        public UserController(IUserBusinessService userBusinessService)
        {
            _userBusinessService = userBusinessService;
        }

        [HttpPost("register")]
        public IActionResult Register([FromBody] UserRegistrationDto model)
        {
            var result = _userBusinessService.Register(model);
            return string.IsNullOrEmpty(result) ? Ok() : BadRequest(result);
        }
        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginUserDto model)
        {
            var result = _userBusinessService.Login(model);
            return Ok(result);
        }
        [HttpPost("refresh-token")]
        public IActionResult RefreshToken([FromBody] RefreshTokenDto model)
        {
            var result = _userBusinessService.RefreshToken(model);
            return string.IsNullOrEmpty(result) ? Forbid() : Ok(new RefreshTokenDto
            {
                RefreshToken = result
            });
        }
    }
}
