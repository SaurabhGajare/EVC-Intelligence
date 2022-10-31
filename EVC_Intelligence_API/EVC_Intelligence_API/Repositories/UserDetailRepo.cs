using EVC_Intelligence_API.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EVC_Intelligence_API.Repositories
{
    public class UserDetailRepo : IUserDetail
    {
        private readonly EVC_IntelligenceContext _context;

        public UserDetailRepo(EVC_IntelligenceContext context)
        {
            _context = context;
        }

        // Add User to DB
        public async Task<UserDetail> Create(UserDetail user)
        {
            _context.UserDetails.Add(user);
            await _context.SaveChangesAsync();

            return user;
        }

        // Get List of user's from UserDetail table
        public async Task<IEnumerable<UserDetail>> Get()
        {
            return await _context.UserDetails.ToListAsync();
        }

        // Get Usr Details by email ID
        public async Task<UserDetail> Get(string emailid)
        {
            return await _context.UserDetails.FindAsync(emailid);
        }

        // Get User Details by userId
        public async Task<UserDetail> GetUserById(int id)
        {
            return await _context.UserDetails.FirstAsync(x => x.UserId == id);
        }

        // Update Phone number of user
        public async Task<UserDetail> UpdatePhone(string phone, int userid)
        {
            var user = await _context.UserDetails.FirstAsync(x => x.UserId == userid);
            if(user != null)
            {
                user.PhoneNumber = long.Parse(phone);
                _context.SaveChanges();
            }

            return user;
        }

        // Fetch all admin details using login detail
        public async Task<List<UserDetail>> Login_Data_Details()
        {
            List<UserDetail> Details = new List<UserDetail>();
            var id = 2;
            var AdminsList = await _context.LoginData.ToListAsync();//.FindAll(x => x.RoleId == id);
            var Admins = AdminsList.FindAll(x => x.RoleId == id);
            foreach (var j in Admins)
            {
                var I = j.UserId;
                var Admin = _context.UserDetails.ToList().Find(x => x.UserId == I);
                Details.Add(Admin);
            }
            return Details;     // Returning all admin data
        }

        // Deleting admin details, by using user id 
        public async Task Delete_Admin_Details(int id)
        {
            var Admin = _context.UserDetails.ToList().Find(x => x.UserId == id);
            var AdminLogin = _context.LoginData.ToList().Find(x => x.UserId == id);

            _context.UserDetails.Remove(Admin);
            _context.LoginData.Remove(AdminLogin);

            await _context.SaveChangesAsync();
        }
    }
}
