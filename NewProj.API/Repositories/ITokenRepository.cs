using Microsoft.AspNetCore.Identity;

namespace NewProj.API.Repositories
{
    public interface ITokenRepository
    {
        string CreateJWTToken(IdentityUser user, List<string> roles);
    }
}
