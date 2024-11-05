using DemoApp.DataAccessLayer.Entities.Abstract;

namespace DemoApp.DataAccessLayer.Entities;

public class UserLogin : EntityBase
{
    public DateTime LoginAt { get; set; } = DateTime.UtcNow;
    public string IpAddress { get; set; }

    public Guid UserId { get; set; }
    public User User { get; set; }
}