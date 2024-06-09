﻿namespace Valuator.Caches.ValueObjects;

public class TextId
{
    public readonly string Value;

    public static TextId New() => new( Guid.NewGuid().ToString() );

    public TextId( string id )
    {
        if ( !id.StartsWith( "TEXT" ) )
        {
            id = $"TEXT-{id}";
        }

        Value = id;
    }

    public override bool Equals( object? obj ) => obj is TextId other && other.Value.Equals( Value );

    public override int GetHashCode() => Value.GetHashCode();

    public override string ToString() => Value;
}