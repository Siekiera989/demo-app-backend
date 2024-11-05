using DemoApp.DataAccessLayer.Entities.Abstract;

namespace DemoApp.DataAccessLayer.Entities;

public class User : EntityBase
{
    public string UserName { get; set; }
    public string Email { get; set; }
    public string PasswordHash { get; set; }
    public bool IsActive { get; set; } = true;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? LastLogin { get; set; }

    public ICollection<UserRole> UserRoles { get; set; } = new List<UserRole>();
    public ICollection<RefreshToken> RefreshTokens { get; set; } = new List<RefreshToken>();
    public ICollection<UserLogin> Logins { get; set; } = new List<UserLogin>();
}