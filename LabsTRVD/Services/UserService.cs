// Services/UserService.cs
using LabsTRVD.DTOs;
using LabsTRVD.Entities;
using LabsTRVD.Interfaces;
using LabsTRVD.Repositories.Interfaces;

namespace LabsTRVD.Services;

public class UserService : IUserService
{
    private readonly IRepository<User> _userRepository;

    public UserService(IRepository<User> userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<RegisterResponseDto> RegisterAsync(RegisterRequestDto dto)
    {
        var existing = await _userRepository.FindAsync(u => u.Email == dto.Email);
        if (existing.Any())
            throw new InvalidOperationException("Користувач з таким email вже існує.");

        string passwordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password);

        var user = new User
        {
            Email = dto.Email,
            PasswordHash = passwordHash,
            Role = "User",
            CreatedAt = DateTime.UtcNow
        };

        await _userRepository.AddAsync(user);

        return new RegisterResponseDto
        {
            UserId = user.UserId,
            Email = user.Email,
            Role = user.Role,
            CreatedAt = user.CreatedAt
        };
    }
}