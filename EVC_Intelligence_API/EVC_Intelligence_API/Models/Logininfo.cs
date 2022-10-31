using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EVC_Intelligence_API.Models
{
    public class Logininfo
    {
        public string email { get; set; }
        public string password { get; set; }
        public string returnurl { get; set; }
        public string name { get; set; }
    }
}
