using FluentAssertions;

namespace Valuator.Domain.Countries;

public partial class Country( string value )
{
    public readonly string Value = value
        .ThrowIfNullOrEmpty()
        .ToUpper();

    public override string ToString()
    {
        return Value;
    }

    public override bool Equals( object? obj )
    {
        return obj is Country other && other.Value == Value;
    }

    public override int GetHashCode()
    {
        return Value.GetHashCode();
    }
}