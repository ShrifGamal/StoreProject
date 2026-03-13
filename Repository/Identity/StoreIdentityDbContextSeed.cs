using Microsoft.AspNetCore.Identity;
using Store.Core.Entites.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Repository.Identity
{
    public static class StoreIdentityDbContextSeed
    {
        public async static Task SeedAppUserAsync(UserManager<AppUser> _userManager)
        {
            if (_userManager.Users.Count() == 0)
            {
                var user = new AppUser()
                {
                    Email = "ShrifGamal@gmali.com",
                    UserName = "Shrif.Gamal",
                    DesplayName = "ShrifGamal",
                    PhoneNumber = "0104567890",
                    Address = new Address()
                    {
                        FName = "Shrif",
                        LName = "Gamal",
                        City = "Cairo",
                        Street = "ElShabab",
                        Country = "Egypt",


                    }

                };

                await _userManager.CreateAsync(user, "P@ssW0rd");
            }
        }
    }
}
