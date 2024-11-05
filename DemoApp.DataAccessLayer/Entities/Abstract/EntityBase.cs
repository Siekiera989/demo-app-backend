using System.ComponentModel.DataAnnotations;

namespace DemoApp.DataAccessLayer.Entities.Abstract;

public class EntityBase
{
    [Key] public Guid Id { get; set; } = Guid.NewGuid();
}