using DyersCargoTransit_API.Model;

namespace DyersCargoTransit_API.Services
{
    public interface IAuthService
    {
        Task<bool> Register(User user);
        Task<bool> Login(User user);
        Task<bool> AssignRoles(string userName, IEnumerable<string> roles);
        Task<string> GenerateToken(User user);
        
    }
}