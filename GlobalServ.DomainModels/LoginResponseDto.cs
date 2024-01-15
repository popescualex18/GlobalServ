using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GlobalServ.DomainModels
{
    public class LoginResponseDto
    {
        public string Error { get; set; }
        public string ErrorCode { get; set; }
        public string AccessToken { get; set; }
        public string RefreshToken { get; set; }
        public string Role { get; set; }
        public string FullName { get; set; }
    }
}
