using Microsoft.AspNetCore.Identity;
using Shop.Domain.Infrastructure;

using System.Security.Claims;
using System.Threading.Tasks;

namespace Shop.Database
{
    public class UserManager
    {
        private readonly UserManager<IdentityUser> _userManager;

        public UserManager(UserManager<IdentityUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task CreateManagerUser(string userName, string password)
        {
           
        }
    }
}
