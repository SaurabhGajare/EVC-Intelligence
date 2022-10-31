using System;
using System.Collections.Generic;

#nullable disable

namespace EVC_Intelligence_API.Models
{
    public partial class LoginDatum
    {
        public LoginDatum()
        {
            BookingDetails = new HashSet<BookingDetail>();
            UserDetails = new HashSet<UserDetail>();
        }

        public int UserId { get; set; }
        public int? RoleId { get; set; }
        public string Password { get; set; }
        public string SecurityQuestion { get; set; }
        public string Answer { get; set; }

        public virtual UserRole Role { get; set; }
        public virtual ICollection<BookingDetail> BookingDetails { get; set; }
        public virtual ICollection<UserDetail> UserDetails { get; set; }
    }
}
