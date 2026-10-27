using ECommerce.Domain.Contracts;
using ECommerce.Domain.Entities.IdentityModule;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Persistance.Data.DataSeed
{
    public class IdentityDataIntializer : IDataInitializer
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly ILogger<IdentityDataIntializer> _logger;

        public IdentityDataIntializer(UserManager<ApplicationUser> userManager,
                                      RoleManager<IdentityRole> roleManager,
                                      ILogger<IdentityDataIntializer> logger
                                     )
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _logger = logger;
        }


        public async Task InitializeAsync()
        {
            try
            {
                if (!_roleManager.Roles.Any())
                {
                    await _roleManager.CreateAsync(new IdentityRole("Admin"));
                    await _roleManager.CreateAsync(new IdentityRole("SuperAdmin"));
                }

                if (!_userManager.Users.Any())
                {
                    var User01 = new ApplicationUser
                    {
                        DisplayName = "Nourhan Taha",
                        UserName = "Nourhan",
                        Email = "norhantaha43@gmail.com",
                        PhoneNumber = "01276545805",
                    };
                    var User02 = new ApplicationUser
                    {
                        DisplayName = "Menna Ahmed",
                        UserName = "Menna",
                        Email = "Menna@gmail.com",
                        PhoneNumber = "01111111111",
                    };

                    await _userManager.CreateAsync(User01, "Nourhan@1234");
                    await _userManager.CreateAsync(User02, "Menna@1234");

                    await _userManager.AddToRoleAsync(User01, "SuperAdmin");
                    await _userManager.AddToRoleAsync(User02, "Admin");
                } 
            }
            catch (Exception ex)
            {
                _logger.LogError($"An error occurred while seeding identity data : {ex.Message}");
            }
        }
    }
}
