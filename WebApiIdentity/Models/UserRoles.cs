using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApiIdentity.Models
{
    public class UserRoles
    {
        public string UserId { get; set; }
        public string RoleId { get; set; }
        public bool IsSelected { get; set; }
        public string RoleName { get; set; }
    }
}
