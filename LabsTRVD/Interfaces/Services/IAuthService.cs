using LabsTRVD.DTOs.AuthDTOs;

namespace LabsTRVD.Interfaces.Services
{
    public interface IAuthService
    {
        Task<AuthResultDto> SignInAsync(SignInDto dto);
    }
}
