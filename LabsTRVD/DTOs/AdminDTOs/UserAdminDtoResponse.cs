namespace LabsTRVD.DTOs.AdminDTOs
{
    /// <summary>
    /// DTO для адміністратора (без чутливих даних)
    /// </summary>
    public class UserAdminDtoResponse
    {
        public Guid UserId { get; set; }
        public string Email { get; set; } = string.Empty;
        public string Role { get; set; } = string.Empty;
        public bool IsBlocked { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? LastLoginAt { get; set; }
    }
}
