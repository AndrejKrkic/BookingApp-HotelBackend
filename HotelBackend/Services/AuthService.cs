using HotelBackend.Models;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.EntityFrameworkCore;
using System;

namespace HotelBackend.Services
{
    public class AuthService : IAuthService
    {
        private readonly HotelContext _context;
        private readonly IConfiguration _config;

        public AuthService(HotelContext context, IConfiguration config)
        {
            _context = context;
            _config = config;
        }

        public async Task<string> Register(string email, string password)
        {
            if (_context.Users.Any(u => u.Email == email))
                throw new Exception("Email već postoji");

            CreatePasswordHash(password, out byte[] hash, out byte[] salt);

            var user = new User
            {
                Email = email,
                PasswordHash = hash,
                PasswordSalt = salt
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return GenerateToken(user);
        }

        public async Task<string> Login(string email, string password)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == email)
                       ?? throw new Exception("Ne postoji korisnik");

            if (!VerifyPassword(password, user.PasswordHash, user.PasswordSalt))
                throw new Exception("Pogrešna lozinka");

            return GenerateToken(user);
        }

        // Helpers
        private void CreatePasswordHash(string password, out byte[] hash, out byte[] salt)
        {
            using var hmac = new System.Security.Cryptography.HMACSHA512();
            salt = hmac.Key;
            hash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
        }

        private bool VerifyPassword(string password, byte[] hash, byte[] salt)
        {
            using var hmac = new System.Security.Cryptography.HMACSHA512(salt);
            var computed = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            return computed.SequenceEqual(hash);
        }

        private string GenerateToken(User user)
        {
            var claims = new[]
                {
                    new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                    new Claim(ClaimTypes.Email, user.Email)
                    // Dodaj još po potrebi (npr. role)
             };

            var key = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(
                _config["Jwt:Key"]!));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.Now.AddDays(7),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
