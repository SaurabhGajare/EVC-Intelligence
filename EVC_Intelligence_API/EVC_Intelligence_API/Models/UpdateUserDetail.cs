using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EVC_Intelligence_API.Models
{
    public class UpdateUserDetail
    {
        public string password { get; set; }
        public string cpassword { get; set; }
        public string currentPassword { get; set; }
        public string phone { get; set; }
        public int userid { get; set; }

    }
}
