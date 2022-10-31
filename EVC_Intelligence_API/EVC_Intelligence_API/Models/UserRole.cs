using System;
using System.Collections.Generic;

#nullable disable

namespace EVC_Intelligence_API.Models
{
    public partial class UserRole
    {
        public UserRole()
        {
            LoginData = new HashSet<LoginDatum>();
        }

        public int RoleId { get; set; }
        public string RoleName { get; set; }

        public virtual ICollection<LoginDatum> LoginData { get; set; }
    }
}
