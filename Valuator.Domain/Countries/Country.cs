using FluentAssertions;

namespace Valuator.Domain.Countries;

public partial class Country
{
    public readonly string Value;

    public Country( string value )
    {
        Value = value.ThrowIfNullOrEmpty();
    }

    public override string ToString() => Value;

    public override bool Equals( object? obj )
    {
        return obj is Country other && other.Value == Value;
    }

    public override int GetHashCode() => Value.GetHashCode();
}