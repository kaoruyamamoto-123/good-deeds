using Domain.Common;

namespace Domain.Models;

public class Role : Entity<int>
{
    public string Name { get; init; } = null!;
    public ICollection<User> Users { get; init; } = [];

    private Role() { }

    public static Role Create(int id, string name)
    {
        ArgumentOutOfRangeException.ThrowIfLessThan(id, 1);

        var role = new Role
        {
            Id = id,
            Name = name
        };
        return role;
    }
}