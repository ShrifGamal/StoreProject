using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Store.APIs.Errors;
using Store.Core.Entites.Identity;
using System.Security.Claims;

namespace Store.APIs.Extensions
{
    public static class UserManagerExtensions
    {

        public static async Task<AppUser> FindByEmailWithAddressAsync(this UserManager<AppUser> userManager , ClaimsPrincipal User)
        {
            var userEmail = User.FindFirstValue(ClaimTypes.Email);
            if (userEmail == null) return null;

            var user = await userManager.Users.Include(U => U.Address).FirstOrDefaultAsync(U => U.Email == userEmail);
            if (user == null) return null;

            return user;


        }
    }
}
