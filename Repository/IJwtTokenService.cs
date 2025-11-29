using DATN.Model;
using System.Security.Claims;

namespace DATN.Repository
{
    public interface IJwtTokenService
    {
        string CreateToken(NguoiDung user, IEnumerable<string> roles, out DateTime expiresAt);
    }
}
