using Domain.Common;

namespace Domain.Models;

public class Category : Entity<int>
{
    public string Title { get; private init; } = null!;

    private Category() { }

    public static Category Create(int id, string title)
    {
        ArgumentOutOfRangeException.ThrowIfLessThan(id, 1);

        if (string.IsNullOrWhiteSpace(title))
        {
            throw new ArgumentNullException(nameof(title), "Title cannot be empty");
        }

        var category = new Category
        {
            Id = id,
            Title = title,
        };
        
        return category;
    }
}