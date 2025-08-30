

using System.Security.Cryptography;

namespace OnlineStore.Application.Providers;
public class PasswordHasher : IPasswordHasher
{
    private const int SALT_SIZE = 16;
    private const int HASH_SIZE = 32;
    private const int IRERATIONS = 100000;

    private readonly HashAlgorithmName _hashAlgorithm = HashAlgorithmName.SHA512;


    public string Hash(string password)
    {
        byte[] salt = RandomNumberGenerator.GetBytes(SALT_SIZE);
        byte[] hash = Rfc2898DeriveBytes.Pbkdf2(password, salt, IRERATIONS, _hashAlgorithm, HASH_SIZE);


        return $"{Convert.ToHexString(hash)}-{Convert.ToHexString(salt)}";
    }

    public bool Verify(string password, string passwordHash)
    {
        string[] parts = passwordHash.Split('-');
        byte[] hash = Convert.FromHexString(parts[0]);
        byte[] salt = Convert.FromHexString(parts[1]);

        byte[] inputHash = Rfc2898DeriveBytes.Pbkdf2(password, salt, IRERATIONS, _hashAlgorithm, HASH_SIZE);

        return CryptographicOperations.FixedTimeEquals(hash, inputHash);
    }


}
