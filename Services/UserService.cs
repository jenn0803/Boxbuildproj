using BoxBuildproj.Areas.Identity.Data;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;

namespace BoxBuildproj.Services
{

    public class UserService
    {
        private readonly UserManager<BoxBuildprojUser> _userManager;

        // Inject UserManager via constructor
        public UserService(UserManager<BoxBuildprojUser> userManager)
        {
            _userManager = userManager;
        }

        // Example method to create a user
        public async Task<IdentityResult> CreateUserAsync(string email, string password)
        {
            var user = new BoxBuildprojUser { UserName = email, Email = email };
            return await _userManager.CreateAsync(user, password);
        }
    }
}