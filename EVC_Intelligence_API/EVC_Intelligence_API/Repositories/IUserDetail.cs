using EVC_Intelligence_API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EVC_Intelligence_API.Repositories
{
    public interface IUserDetail
    {
        Task<IEnumerable<UserDetail>> Get();
        Task<UserDetail> Get(string emailid);
        Task<UserDetail> GetUserById(int id);
        Task<UserDetail> Create(UserDetail user);
        Task<UserDetail> UpdatePhone(string phone, int userid);
        Task<List<UserDetail>> Login_Data_Details();
        Task Delete_Admin_Details(int id);
    }
}
