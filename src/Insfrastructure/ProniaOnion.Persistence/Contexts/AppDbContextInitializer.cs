using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using ProniaOnion.Domain.Entities;
using ProniaOnion.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProniaOnion.Persistence.Contexts
{
    public class AppDbContextInitializer
    {
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<AppUser> _userManager;
        private readonly IConfiguration _configuration;
        private readonly AppDbContext _context;

        public AppDbContextInitializer(RoleManager<IdentityRole> roleManager,
            UserManager<AppUser> userManager,
            IConfiguration confoguration
            ,AppDbContext context)
        {
            _roleManager = roleManager;
            _userManager = userManager;
            _configuration = confoguration;
            _context = context;
        }
        public async Task InitializeDbContext()
        {
            await _context.Database.MigrateAsync();
        }

        public async Task CreateRolesAsync()
        {
            foreach (UserRole role in Enum.GetValues(typeof(UserRole)))
            {
                if(!await _roleManager.RoleExistsAsync(role.ToString()))
                {
                    await _roleManager.CreateAsync(new IdentityRole {Name = role.ToString()});
                }
            }
        }


        public async Task InitializeAdmin()
        {
            AppUser admin = new AppUser
            {
                Name = "Admin",
                Surname = "Admin",
                Email = _configuration["AdminSettings:Email"],
                UserName = _configuration["AdminSettings:Username"]
            };


            await _userManager.CreateAsync(admin, _configuration["AdminSettings:Password"]);
            await _userManager.AddToRoleAsync(admin,UserRole.Admin.ToString());

        }



    }
}
