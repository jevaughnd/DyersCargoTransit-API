using DyersCargoTransit_API.Model;

namespace DyersCargoTransit_API.Services
{
    public interface IAuthService
    {
        Task<bool> RegisterUser(AppUser user);
        Task<bool> LoginUser(AppUser user);
        Task<bool> AssignRoles(string userName, IEnumerable<string> roles);
        Task<string> GenerateToken(AppUser user);
      
    }
}