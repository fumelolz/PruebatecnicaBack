using PruebatecnicaBack.Application.Common.Interfaces.Authentication;
using PruebatecnicaBack.Application.Common.Services;
using PruebatecnicaBack.Domain.Entities;
using PruebatecnicaBack.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace PruebatecnicaBack.Infrastructure.Authentication
{
    public class JwtTokenGenerator : IJwtTokenGenerator
    {
        private readonly JwtSettings _jwtOptions;
        private readonly IDateTimeProvider _dateTimeProvider;
        private readonly ApplicationDbContext _dbContext;

        public JwtTokenGenerator(IDateTimeProvider dateTimeProvider, IOptions<JwtSettings> jwtOptions, ApplicationDbContext dbContext)
        {
            this._dateTimeProvider=dateTimeProvider;
            _jwtOptions=jwtOptions.Value;
            _dbContext=dbContext;
        }

        public async Task<string> GenerateToken(User user)
        {
            var signingCredentials = new SigningCredentials(
                new SymmetricSecurityKey(
                    Encoding.UTF8.GetBytes(_jwtOptions.Secret)),
                SecurityAlgorithms.HmacSha256);

            List<string> rolesNames = await _dbContext.UserRoles.Where(ur => ur.UserId == user.UserId).Select(ur => ur.Role.Name).ToListAsync();


            List<Claim> claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.UserId.ToString()),
                new Claim(JwtRegisteredClaimNames.GivenName, user.Name ?? string.Empty),
                new Claim(JwtRegisteredClaimNames.FamilyName, $"{user.FirstSurname} {user.SecondSurname}".Trim()),
                new Claim(JwtRegisteredClaimNames.Email, user.Email ?? string.Empty),
            };

            claims.AddRange((rolesNames ?? new List<string>()).Select(r => new Claim(ClaimTypes.Role, r)));


            var securityToken = new JwtSecurityToken(
                issuer: _jwtOptions.Issuer,
                audience: _jwtOptions.Audience,
                expires: _dateTimeProvider.UtcNow.AddMinutes(_jwtOptions.ExpirationTimeInMinutes),
                claims: claims,
                signingCredentials: signingCredentials);

            return new JwtSecurityTokenHandler().WriteToken(securityToken);
        }

        public async Task<string> GenerateRefreshToken(User user)
        {
            var signingCredentials = new SigningCredentials(
                new SymmetricSecurityKey(
                    Encoding.UTF8.GetBytes(_jwtOptions.Secret)),
                SecurityAlgorithms.HmacSha256);

            List<string> rolesNames = await _dbContext.UserRoles.Where(ur => ur.UserId == user.UserId).Select(ur => ur.Role.Name).ToListAsync();


            List<Claim> claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.UserId.ToString()),
                new Claim(JwtRegisteredClaimNames.GivenName, user.Name ?? string.Empty),
                new Claim(JwtRegisteredClaimNames.FamilyName, $"{user.FirstSurname} {user.SecondSurname}".Trim()),
                new Claim(JwtRegisteredClaimNames.Email, user.Email ?? string.Empty),
            };

            claims.AddRange((rolesNames ?? new List<string>()).Select(r => new Claim(ClaimTypes.Role, r)));


            var securityToken = new JwtSecurityToken(
                issuer: _jwtOptions.Issuer,
                audience: _jwtOptions.Audience,
                expires: _dateTimeProvider.UtcNow.AddMinutes(10080),
                claims: claims,
                signingCredentials: signingCredentials);

            return new JwtSecurityTokenHandler().WriteToken(securityToken);
        }

    }
}
