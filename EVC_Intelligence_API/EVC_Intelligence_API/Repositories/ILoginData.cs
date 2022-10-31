using EVC_Intelligence_API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EVC_Intelligence_API.Repositories
{
    public interface ILoginData
    {
        Task<IEnumerable<LoginDatum>> Get();
        Task<LoginDatum> Get(int userid);
        Task<LoginDatum> Create(LoginDatum user);
        Task<bool> UpdatePassword(int userid, string password, string currentPassword, string PasswordHash);
    }
}
