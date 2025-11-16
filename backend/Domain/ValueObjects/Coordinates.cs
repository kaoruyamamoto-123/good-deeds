namespace Domain.ValueObjects;

using System;

public sealed class Coordinates
{
    public double Latitude { get; }
    public double Longitude { get; }

    public Coordinates(double latitude, double longitude)
    {
        // Validate ranges
        if (latitude is < -90 or > 90)
        {
            throw new ArgumentOutOfRangeException(nameof(latitude), "Latitude must be between -90 and 90 degrees.");
        }

        if (longitude is < -180 or > 180)
        {
            throw new ArgumentOutOfRangeException(nameof(longitude), "Longitude must be between -180 and 180 degrees.");
        }

        Latitude = latitude;
        Longitude = longitude;
    }

    public override bool Equals(object? obj) => Equals(obj as Coordinates);
    
    public bool Equals(Coordinates? other)
    {
        if (other is null)
        {
            return false;
        }

        if (ReferenceEquals(this, other))
        {
            return true;
        }
        
        return Latitude.Equals(other.Latitude) && Longitude.Equals(other.Longitude);
    }

    public override int GetHashCode() => HashCode.Combine(Latitude, Longitude);

    public override string ToString() => $"({Latitude:N6}, {Longitude:N6})";

    // Operator overloads
    public static bool operator ==(Coordinates? left, Coordinates? right) => Equals(left, right);
    
    public static bool operator !=(Coordinates? left, Coordinates? right) => !Equals(left, right);
}