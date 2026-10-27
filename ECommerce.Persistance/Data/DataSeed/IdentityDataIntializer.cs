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
                if (!await _roleManager.RoleExistsAsync("Admin"))
                {
                    await _roleManager.CreateAsync(new IdentityRole("Admin"));
                }

                if (!await _roleManager.RoleExistsAsync("SuperAdmin"))
                {
                    await _roleManager.CreateAsync(new IdentityRole("SuperAdmin"));
                }

                var user = await _userManager.FindByNameAsync("AyahTablya");

                if (user == null)
                {
                    user = new ApplicationUser
                    {
                        DisplayName = "Ayah Tablya",
                        UserName = "AyahTablya",
                        Email = "ayahtarek2019@gmail.com",
                        PhoneNumber = "01000250943"
                    };

                    var result = await _userManager.CreateAsync(user, "Ayah@1234");

                    if (!result.Succeeded)
                    {
                        _logger.LogError(
                            "Failed to create Ayah: {Errors}",
                            string.Join(", ", result.Errors.Select(e => e.Description)));
                        return;
                    }
                }

                if (!await _userManager.IsInRoleAsync(user, "SuperAdmin"))
                {
                    await _userManager.AddToRoleAsync(user, "SuperAdmin");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while seeding identity data.");
            }
        }

    }

}
