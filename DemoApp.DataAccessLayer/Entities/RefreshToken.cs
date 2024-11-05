using DemoApp.DataAccessLayer.Entities.Abstract;

namespace DemoApp.DataAccessLayer.Entities;

public class RefreshToken : EntityBase
{
    public string Token { get; set; }
    public DateTime ExpiresAt { get; set; }
    public bool IsRevoked { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public Guid UserId { get; set; }
    public User User { get; set; }
}