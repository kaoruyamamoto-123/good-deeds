namespace Domain.ValueObjects;

public class Address
{
    public string City { get; set; }
    public string Street { get; set; }

    
    public Address(string city, string street)
    {
        if (string.IsNullOrWhiteSpace(city))
        {
            throw new ArgumentNullException(nameof(city));
        }

        if (string.IsNullOrWhiteSpace(street))
        {
            throw new ArgumentNullException(nameof(street));
        }
        
        City = city;
        Street = street;
    }
    
    public override bool Equals(object? obj) => Equals(obj as Address);
    
    public bool Equals(Address? other)
    {
        if (other is null)
        {
            return false;
        }

        if (ReferenceEquals(this, other))
        {
            return true;
        }
        
        return City.Equals(other.City) && Street.Equals(other.Street);
    }

    public override int GetHashCode() => HashCode.Combine(City, Street);

    public override string ToString() => $"{City}, {Street}";

    // Operator overloads
    public static bool operator ==(Address? left, Address? right) => Equals(left, right);
    
    public static bool operator !=(Address? left, Address? right) => !Equals(left, right);
}