using System.Security.Cryptography;
using System.Text;

namespace DATN.Utils
{
    public static class PasswordHasher
    {
        public static byte[] HashPassword(string password)
        {
            using var sha256 = SHA256.Create();
            return sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
        }

        public static bool VerifyPassword(string password, byte[]? hash)
        {
            if (hash == null) return false;
            var newHash = HashPassword(password);
            return newHash.SequenceEqual(hash);
        }
    }
}
