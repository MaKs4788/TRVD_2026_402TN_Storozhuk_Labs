using LabsTRVD.DTOs.AuthDTOs;
using LabsTRVD.Entities;
using LabsTRVD.Interfaces.Services;
using LabsTRVD.Settings;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace LabsTRVD.Services;

public class AuthService : IAuthService
{
    private readonly JwtSettings _jwtSettings;
    private readonly IRepository<User> _userRepository;

    public AuthService(IOptions<JwtSettings> jwtSettings, IRepository<User> userRepository)
    {
        _jwtSettings = jwtSettings.Value;
        _userRepository = userRepository;
    }

    public async Task<AuthResultDto> SignInAsync(SignInDto dto)
    {
        var users = await _userRepository.FindAsync(u => u.Email == dto.Email);
        var user = users.FirstOrDefault()
            ?? throw new UnauthorizedAccessException("Невірний email або пароль.");

        if (user.IsBlocked)
            throw new UnauthorizedAccessException("Ваш облік заблокований адміністратором. Зв'яжіться з підтримкою.");

        bool isValid = BCrypt.Net.BCrypt.Verify(dto.Password, user.PasswordHash);
        if (!isValid)
            throw new UnauthorizedAccessException("Невірний email або пароль.");

        user.LastLoginAt = DateTime.Now;
        await _userRepository.UpdateAsync(user);

        string token = GenerateJwtToken(user);

        return new AuthResultDto
        {
            Token = token,
            Email = user.Email,
            Role = user.Role
        };
    }

    private string GenerateJwtToken(User user)
    {
        var key = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(_jwtSettings.SecretKey));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Sub, user.UserId.ToString()),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new Claim(ClaimTypes.Email, user.Email),
            new Claim(ClaimTypes.Role, user.Role)
        };

        var token = new JwtSecurityToken(
            issuer: _jwtSettings.Issuer,
            audience: _jwtSettings.Audience,
            claims: claims,
            expires: DateTime.Now.AddMinutes(_jwtSettings.ExpiryMinutes), // ✅ термін дії
            signingCredentials: creds
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}