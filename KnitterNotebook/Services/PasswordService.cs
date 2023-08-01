using KnitterNotebook.Services.Interfaces;

namespace KnitterNotebook.Services
{
    public class PasswordService : IPasswordService
    {
        /// <summary>
        /// Verifies that the hash of given password matches hashed password
        /// </summary>
        /// <param name="password">Unhashed password</param>
        /// <param name="hash">Hashed password</param>
        /// <returns>True if password matches, otherwise false</returns>
        /// <exception cref="ArgumentNullException"></exception>
        public bool VerifyPassword(string password, string hash) => BCrypt.Net.BCrypt.Verify(password, hash);

        /// <summary>
        /// Hashes password
        /// </summary>
        /// <param name="password">Unhashed password</param>
        /// <returns>Hashed password</returns>
        /// <exception cref="ArgumentNullException"></exception>
        public string HashPassword(string password) => BCrypt.Net.BCrypt.HashPassword(password);
    }
}