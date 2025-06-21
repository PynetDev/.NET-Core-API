using WebApplication1.Model;

namespace WebApplication1.Services
{
    public interface IJwtTokenService
    {
        string GenerateToken(User user);

    }
}
