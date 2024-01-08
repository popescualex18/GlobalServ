﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GlobalServ.DataModels.Models
{
    public class UserModel
    {
        public Guid Id { get; set; }
        public string FullName { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public string Name { get; set; }
        public string PhoneNumber { get; set; }
        public int RoleId { get; set; }

        public RoleModel Role { get; set; }
    }
}
