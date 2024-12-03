using System;
using HealthCareABApi.Models;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace HealthCareABApi.Services
{
    // This class is responsible for generating JWT tokens.
    public class JwtTokenService
    {
        // Private fields to store JWT configuration settings.
        private readonly string _secret;
        private readonly string _issuer;
        private readonly string _audience;
        private readonly int _expiryInMinutes;

        // Constructor that initializes the JWT settings from the application's configuration.
        // The settings are defined in appsettings.json.
        public JwtTokenService(IConfiguration configuration)
        {
            // Retrieve the JWT secret key, issuer, audience, and token expiry duration from configuration.
            _secret = configuration["JwtSettings:Secret"];
            _issuer = configuration["JwtSettings:Issuer"];
            _audience = configuration["JwtSettings:Audience"];
            _expiryInMinutes = int.Parse(configuration["JwtSettings:ExpiryInMinutes"]);
        }

        // Method to generate a JWT token for a given user.
        public string GenerateToken(User user)
        {
            // Create a list of claims for the JWT token.
            // Claims are key-value pairs that represent the user's identity and roles.
            var claims = new List<Claim>
        {
            // Add a claim with the user's username.
            new Claim(ClaimTypes.Name, user.Username),
            // Add a unique identifier (JTI) claim for this token instance.
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),

            // Add the user's ID as a custom claim.
            new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()) // 'sub' (subject) is commonly used for user IDs in JWTs.
        };

            // Add claims for each of the user's roles.
            // This allows role-based access control by embedding roles in the JWT token.
            claims.AddRange(user.Roles.Select(role => new Claim(ClaimTypes.Role, role)));

            // Create a security key from the secret, encoded as UTF-8.
            // This key is used to sign the token, ensuring its authenticity.
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_secret));

            // Create signing credentials using the key and specifying HMAC SHA-256 as the signing algorithm.
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            // Generate the JWT token with specified properties:
            var token = new JwtSecurityToken(
                _issuer,
                _audience,
                claims,
                expires: DateTime.Now.AddMinutes(_expiryInMinutes),
                signingCredentials: creds);

            // Return the token as a serialized string.
            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }

}