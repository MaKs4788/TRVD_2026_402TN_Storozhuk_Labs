using LabsTRVD.DTOs;
using LabsTRVD.Entities;

namespace LabsTRVD.Interfaces
{
    public interface IUserService
    {
        Task<RegisterResponseDto> RegisterAsync(RegisterRequestDto dto);
    }
}
