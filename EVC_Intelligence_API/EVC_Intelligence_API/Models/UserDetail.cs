using System;
using System.Collections.Generic;

#nullable disable

namespace EVC_Intelligence_API.Models
{
    public partial class UserDetail
    {
        public int UserId { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public long? PhoneNumber { get; set; }
        public int? Coins { get; set; }

        public virtual LoginDatum User { get; set; }
    }
}
