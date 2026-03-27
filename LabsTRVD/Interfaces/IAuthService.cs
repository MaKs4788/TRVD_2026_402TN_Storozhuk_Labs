using LabsTRVD.DTOs;

namespace LabsTRVD.Interfaces
{
    public interface IAuthService
    {
        Task<AuthResultDto> SignInAsync(SignInDto dto);
    }
}
