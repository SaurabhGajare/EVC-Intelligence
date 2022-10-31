using EVC_Intelligence_API.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EVC_Intelligence_API.Repositories
{
    public class LoginDataRepo : ILoginData
    {
        private readonly EVC_IntelligenceContext _context;

        public LoginDataRepo(EVC_IntelligenceContext context)
        {
            _context = context;
        }

        // Add User to DB
        public async Task<LoginDatum> Create(LoginDatum user)
        {
            _context.LoginData.Add(user);
            await _context.SaveChangesAsync();

            return user;
        }

        // Get List of Users
        public async Task<IEnumerable<LoginDatum>> Get()
        {
            return await _context.LoginData.ToListAsync();
        }

        // Get user by userId
        public async Task<LoginDatum> Get(int userid)
        {
            return await _context.LoginData.FindAsync(userid);
        }

        // Update Password of User in DB
        public async Task<bool> UpdatePassword(int userid, string password, string currentPassword, string passwordHash)
        {
            var result = false;
            var verify = BCrypt.Net.BCrypt.Verify(currentPassword, passwordHash);
            if (verify)
            {
                var user = await _context.LoginData.FirstAsync(x => x.UserId == userid);

                if (user != null)
                {
                    user.Password = BCrypt.Net.BCrypt.HashPassword(password);
                    _context.SaveChanges();
                }
                result = true;
            }

            return result;
        }
    }
}
