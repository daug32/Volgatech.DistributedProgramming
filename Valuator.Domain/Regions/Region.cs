using FluentAssertions;

namespace Valuator.Domain.Regions;

public partial class Region( string value )
{
    public readonly string Value = value
        .ThrowIfNullOrEmpty()
        .ToUpper();

    public override string ToString() => Value;

    public override bool Equals( object? obj ) => obj is Region other && other.Value == Value;

    public override int GetHashCode() => Value.GetHashCode();
}