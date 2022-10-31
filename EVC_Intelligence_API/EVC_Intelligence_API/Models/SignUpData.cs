using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EVC_Intelligence_API.Models
{
    public class SignUpData
    {
        public string name { get; set; }
        public string email { get; set; }
        public string password { get; set; }
        public string cpassword { get; set; }
        public long phone { get; set; }
        public string question { get; set; }
        public string answer { get; set; }
    }
}
