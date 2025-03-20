using System.Security.Cryptography;
using System.Text;

namespace Lagkage.Contracts.Models;

public static class GuidGenerator
{
    public static Guid CreateGuidFromString(string input)
    {
        using (var md5 = MD5.Create())
        {
            // Compute the hash from the input string
            byte[] hashBytes = md5.ComputeHash(Encoding.UTF8.GetBytes(input));

            // Use the first 16 bytes of the hash to create a Guid
            return new Guid(hashBytes);
        }
    }
}