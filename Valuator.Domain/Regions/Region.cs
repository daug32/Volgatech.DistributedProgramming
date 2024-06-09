namespace Valuator.Domain.Regions;

public partial class Region
{
    public readonly string Value;

    public Region( string value )
    {
        Value = value;
    }

    public override string ToString() => Value;

    public override bool Equals( object? obj )
    {
        return obj is Region other && other.Value == Value;
    }

    public override int GetHashCode() => Value.GetHashCode();
}