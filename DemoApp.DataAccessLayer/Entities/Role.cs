using DemoApp.DataAccessLayer.Entities.Abstract;

namespace DemoApp.DataAccessLayer.Entities;

public class Role : EntityBase
{
    public string Name { get; set; }

    public ICollection<UserRole> UserRoles { get; set; } = new List<UserRole>();
}