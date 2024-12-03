using HealthCareABApi.Models;
using HealthCareABApi.Repositories.Interfaces;
using BCrypt.Net;
using Microsoft.EntityFrameworkCore;

namespace HealthCareABApi.Services
{

	public class UserService
    {
        private readonly IAppDbContext _context;

        public UserService(IAppDbContext context)
        {
            _context = context;
        }

        public async Task<bool> ExistsByUsernameAsync(string username)
		{
			return await _context.Users.AnyAsync(u => u.Username == username);
        }

        public async Task<User> GetUserByUsernameAsync(string username)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.Username == username);
        }

        public async Task CreateUserAsync(User user)
        {
			await _context.Users.AddAsync(user);
			await _context.SaveChangesAsync();
		}

        // Method to hash a plaintext password using BCrypt.
        public string HashPassword(string password)
        {
            // Hash the password and return the hashed string.
            // BCrypt automatically generates a salt and applies it to the password, adding strong security to the hash.
            return BCrypt.Net.BCrypt.HashPassword(password);
        }

        // Method to verify a plaintext password against a hashed password.
        public bool VerifyPassword(string enteredPassword, string storedHash)
        {
            // Check if the entered password, when hashed, matches the stored hash.
            // BCrypt compares the entered password with the hashed password and returns true if they match.
            return BCrypt.Net.BCrypt.Verify(enteredPassword, storedHash);
        }
    }

}