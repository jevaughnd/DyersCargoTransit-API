using DyersCargoTransit_API.Model;

namespace DyersCargoTransit_API.Services
{
    public interface IAuthService
    {
        Task<bool> RegisterUser(User user);
        Task<bool> LoginUser(User user);
        Task<bool> AssignRoles(string userName, IEnumerable<string> roles);
        Task<string> GenerateToken(User user);
      
    }
}